using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Domain.Interfaces;
using System.Transactions;

namespace SubastaYa.API.BackgroundServices;

public class AuctionClosingWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AuctionClosingWorker> _logger;
    private readonly TimeSpan _intervaloRevision = TimeSpan.FromSeconds(15); // Revisa cada 15 segundos

    public AuctionClosingWorker(IServiceScopeFactory scopeFactory, ILogger<AuctionClosingWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[Worker] Servicio de cierre de subastas iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcesarSubastasVencidasAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Worker] Error al procesar subastas vencidas: {Message}", ex.Message);
            }

            await Task.Delay(_intervaloRevision, stoppingToken);
        }
    }

    private async Task ProcesarSubastasVencidasAsync()
    {
        using var scope = _scopeFactory.CreateScope();

        var subastaRepo = scope.ServiceProvider.GetRequiredService<ISubastaRepository>();
        var pujaRepo = scope.ServiceProvider.GetRequiredService<IPujaRepository>();
        var billeteraRepo = scope.ServiceProvider.GetRequiredService<IBilleteraRepository>();
        var ledgerRepo = scope.ServiceProvider.GetRequiredService<ITransaccionLedgerRepository>();
        var auditoriaRepo = scope.ServiceProvider.GetRequiredService<IAuditoriaLogRepository>();

        var ahora = DateTime.UtcNow;

        // 1. Buscar subastas activas cuya fecha de fin ya haya pasado
        var subastas = await subastaRepo.ObtenerTodasAsync();
        var subastasVencidas = subastas
            .Where(s => s.Estado == EstadoSubasta.Activa && s.FechaFin <= ahora)
            .ToList();

        if (!subastasVencidas.Any())
            return;

        _logger.LogInformation("[Worker] Se encontraron {Cantidad} subastas vencidas para procesar.", subastasVencidas.Count);

        foreach (var subasta in subastasVencidas)
        {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var pujaGanadora = await pujaRepo.ObtenerPujaMasAltaAsync(subasta.Id);

            if (pujaGanadora != null)
            {
                // =========================================================================
                // CASO CON GANADOR: LIQUIDACIÓN FINAL ATÓMICA
                // =========================================================================
                _logger.LogInformation("[Worker] Subasta ID {SubastaId} finalizada con ganador Usuario ID {GanadorId} por ${Monto:N2}.", 
                    subasta.Id, pujaGanadora.CompradorId, pujaGanadora.Monto);

                // A. Marcar subasta como FINALIZADA
                subasta.Estado = EstadoSubasta.Finalizada;
                subasta.Version++;
                await subastaRepo.ActualizarAsync(subasta);

                // B. Débito final del comprador (su dinero ya estaba retenido, ahora sale del total)
                var billeteraComprador = await billeteraRepo.ObtenerPorUsuarioIdAsync(pujaGanadora.CompradorId);
                if (billeteraComprador != null)
                {
                    billeteraComprador.SaldoRetenido -= pujaGanadora.Monto;
                    billeteraComprador.SaldoTotal -= pujaGanadora.Monto;
                    billeteraComprador.Version++;

                    await billeteraRepo.ActualizarAsync(billeteraComprador);

                    // Registrar asiento de PAGO en el Ledger del comprador
                    await ledgerRepo.AgregarAsync(new TransaccionLedger
                    {
                        BilleteraId = billeteraComprador.Id,
                        Tipo = TipoTransaccionLedger.Pago,
                        Monto = pujaGanadora.Monto,
                        Fecha = ahora,
                        SubastaId = subasta.Id
                    });
                }

                // C. Acreditación al vendedor (se suma a su total y a su disponible)
                var billeteraVendedor = await billeteraRepo.ObtenerPorUsuarioIdAsync(subasta.VendedorId);
                if (billeteraVendedor != null)
                {
                    billeteraVendedor.SaldoTotal += pujaGanadora.Monto;
                    billeteraVendedor.SaldoDisponible += pujaGanadora.Monto;
                    billeteraVendedor.Version++;

                    await billeteraRepo.ActualizarAsync(billeteraVendedor);

                    // Registrar asiento de COBRO en el Ledger del vendedor
                    await ledgerRepo.AgregarAsync(new TransaccionLedger
                    {
                        BilleteraId = billeteraVendedor.Id,
                        Tipo = TipoTransaccionLedger.Cobro,
                        Monto = pujaGanadora.Monto,
                        Fecha = ahora,
                        SubastaId = subasta.Id
                    });
                }

                // D. Auditoría obligatoria de liquidación por Worker
                await auditoriaRepo.AgregarAsync(new AuditoriaLog
                {
                    Entidad = "SUBASTA",
                    EntidadId = subasta.Id,
                    Accion = "CIERRE_FINALIZADA",
                    UsuarioId = null, // NULL porque fue ejecutado automáticamente por el Worker
                    DetalleJson = $"{{\"compradorId\": {pujaGanadora.CompradorId}, \"vendedorId\": {subasta.VendedorId}, \"montoFinal\": {pujaGanadora.Monto}}}",
                    Fecha = ahora
                });
            }
            else
            {
                // =========================================================================
                // CASO SIN OFERTAS: PASA A DESIERTA
                // =========================================================================
                _logger.LogInformation("[Worker] Subasta ID {SubastaId} venció sin ofertas. Marcada como DESIERTA.", subasta.Id);

                subasta.Estado = EstadoSubasta.Desierta;
                subasta.Version++;
                await subastaRepo.ActualizarAsync(subasta);

                await auditoriaRepo.AgregarAsync(new AuditoriaLog
                {
                    Entidad = "SUBASTA",
                    EntidadId = subasta.Id,
                    Accion = "CIERRE_DESIERTA",
                    UsuarioId = null,
                    DetalleJson = "{\"motivo\": \"Vencimiento sin ofertas registradas\"}",
                    Fecha = ahora
                });
            }

            tx.Complete();
        }
    }
}