using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace SubastaYa.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(SubastaYaDbContext context)
        {
            // 1. Si ya existen usuarios en la base de datos, no volvemos a sembrar nada
            if (await context.Usuarios.AnyAsync()) return;

            var ahora = DateTime.UtcNow;
            string passwordHashDefault = "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G";

            // 2. Usuarios Semilla
            var usuarios = new[]
            {
                new Usuario { Id = 1, Nombre = "Vendedor", Email = "vendedor@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora.AddDays(-10) },
                new Usuario { Id = 2, Nombre = "Comprador 1", Email = "comprador1@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora.AddDays(-10) },
                new Usuario { Id = 3, Nombre = "Comprador 2", Email = "comprador2@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora.AddDays(-10) },
                new Usuario { Id = 4, Nombre = "Sin Fondos", Email = "sinfondos@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora.AddDays(-10) }
            };
            await context.Usuarios.AddRangeAsync(usuarios);

            // 3. Billeteras Semilla
            var billeteras = new[]
            {
                new Billetera { Id = 1, UsuarioId = 1, SaldoTotal = 0, SaldoRetenido = 0, SaldoDisponible = 0, Version = 1 },
                new Billetera { Id = 2, UsuarioId = 2, SaldoTotal = 150000m, SaldoRetenido = 45000m, SaldoDisponible = 105000m, Version = 1 },
                new Billetera { Id = 3, UsuarioId = 3, SaldoTotal = 200000m, SaldoRetenido = 0, SaldoDisponible = 200000m, Version = 1 },
                new Billetera { Id = 4, UsuarioId = 4, SaldoTotal = 500m, SaldoRetenido = 0, SaldoDisponible = 500m, Version = 1 }
            };
            await context.Billeteras.AddRangeAsync(billeteras);

            // 4. Categorías
            var categorias = new[]
            {
                new Categoria { Id = 1, Nombre = "Tecnología", UrlIcono = "icon-tech" },
                new Categoria { Id = 2, Nombre = "Coleccionables", UrlIcono = "icon-col" },
                new Categoria { Id = 3, Nombre = "Indumentaria", UrlIcono = "icon-ropa" },
                new Categoria { Id = 4, Nombre = "Vehículos", UrlIcono = "icon-auto" }
            };
            await context.Categorias.AddRangeAsync(categorias);

            // 5. Subastas con fechas calculadas en tiempo de ejecución
            var subastas = new[]
            {
                // Caso A: Activa estándar (cierra en 7 días)
                new Subasta { Id = 1, VendedorId = 1, CategoriaId = 1, Titulo = "PlayStation 5", Descripcion = "SSD alta velocidad 825GB 4K Ray Tracing.", UrlImagen = "https://i.ibb.co/jPwDCZ16/descarga-2026-03-31-T111030-325.png", PrecioBase = 10000m, IncrementoMinimo = 1000m, FechaInicio = ahora.AddDays(-1), FechaFin = ahora.AddDays(7), Estado = EstadoSubasta.Activa, Version = 1 },
                
                // Caso B: Activa crítica (cierra en 15 días)
                new Subasta { Id = 2, VendedorId = 1, CategoriaId = 1, Titulo = "Apple iPhone 13", Descripcion = "128 GB Azul, A15 Bionic.", UrlImagen = "https://i.ibb.co/NvpNhBw/2b9cec46-e3fb-4fd3-b83e-6093aa11f3ef.webp", PrecioBase = 5000m, IncrementoMinimo = 500m, FechaInicio = ahora.AddDays(-1), FechaFin = ahora.AddDays(15), Estado = EstadoSubasta.Activa, Version = 1 },
                
                // Caso C: Próxima (inicia en 2 días)
                new Subasta { Id = 3, VendedorId = 1, CategoriaId = 2, Titulo = "Figura De Accion Articulada", Descripcion = "Figuras articuladas 30 cm.", UrlImagen = "https://i.ibb.co/SXyKgwsB/200240-800-auto.webp", PrecioBase = 20000m, IncrementoMinimo = 2000m, FechaInicio = ahora.AddDays(2), FechaFin = ahora.AddDays(10), Estado = EstadoSubasta.Programada, Version = 1 },
                
                // Caso D: Vencida con ganador (venció ayer)
                new Subasta { Id = 4, VendedorId = 1, CategoriaId = 3, Titulo = "Camiseta de Fútbol", Descripcion = "Camiseta oficial Boca Juniors.", UrlImagen = "https://i.ibb.co/chydTrMW/JJ4286-01.webp", PrecioBase = 30000m, IncrementoMinimo = 1500m, FechaInicio = ahora.AddDays(-5), FechaFin = ahora.AddDays(-1), Estado = EstadoSubasta.Activa, Version = 1 },
                
                // Caso E: Vencida desierta (venció hace 2 días)
                new Subasta { Id = 5, VendedorId = 1, CategoriaId = 4, Titulo = "Bicicleta Kanji R/29", Descripcion = "Bicicleta rodado 29.", UrlImagen = "https://i.ibb.co/d4yNsNSs/840-kaj-289.webp", PrecioBase = 50000m, IncrementoMinimo = 5000m, FechaInicio = ahora.AddDays(-5), FechaFin = ahora.AddDays(-2), Estado = EstadoSubasta.Activa, Version = 1 }
            };
            await context.Subastas.AddRangeAsync(subastas);

            // 6. Trazabilidad contable completa en el Ledger (Conciliada)
            var transacciones = new[]
            {
                new TransaccionLedger { Id = 1, BilleteraId = 2, Tipo = TipoTransaccionLedger.Deposito, Monto = 150000m, Fecha = ahora.AddDays(-5) },
                new TransaccionLedger { Id = 2, BilleteraId = 2, Tipo = TipoTransaccionLedger.Retencion, Monto = 45000m, Fecha = ahora.AddHours(-1), SubastaId = 1 },

                // Historial conciliado de Comprador 2 (ID 3)
                new TransaccionLedger { Id = 3, BilleteraId = 3, Tipo = TipoTransaccionLedger.Deposito, Monto = 200000m, Fecha = ahora.AddDays(-5) },
                new TransaccionLedger { Id = 4, BilleteraId = 3, Tipo = TipoTransaccionLedger.Retencion, Monto = 40000m, Fecha = ahora.AddHours(-3), SubastaId = 1 },
                new TransaccionLedger { Id = 5, BilleteraId = 3, Tipo = TipoTransaccionLedger.Liberacion, Monto = 40000m, Fecha = ahora.AddHours(-1), SubastaId = 1 },

                new TransaccionLedger { Id = 6, BilleteraId = 4, Tipo = TipoTransaccionLedger.Deposito, Monto = 500m, Fecha = ahora.AddDays(-5) }
            };
            await context.TransaccionesLedger.AddRangeAsync(transacciones);

            // 7. Pujas
            var pujas = new[]
            {
                new Puja { Id = 1, SubastaId = 1, CompradorId = 3, Monto = 40000m, FechaPuja = ahora.AddHours(-3) },
                new Puja { Id = 2, SubastaId = 1, CompradorId = 2, Monto = 45000m, FechaPuja = ahora.AddHours(-1) },
                new Puja { Id = 3, SubastaId = 4, CompradorId = 2, Monto = 35000m, FechaPuja = ahora.AddDays(-2) }
            };
            await context.Pujas.AddRangeAsync(pujas);

            // Guardar cambios en la BD
            await context.SaveChangesAsync();
        }
    }
}
