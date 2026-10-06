using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Subastas;
using SubastaYa.Domain.Interfaces;

namespace SubastaYa.Application.Queries.Subastas
{
    public class ObtenerCatalogoQueryHandler : IQueryHandler<ObtenerCatalogoQuery, ResultadoPaginadoDto<SubastaDto>>
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerCatalogoQueryHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<ResultadoPaginadoDto<SubastaDto>> HandleAsync(ObtenerCatalogoQuery query, CancellationToken cancellationToken = default)
        {
            // UNA sola query con Include(Categoria, Vendedor, Pujas): sin N+1
            var subastas = await _subastaRepository.ObtenerTodasAsync();

            var filtradas = subastas.AsQueryable();
            if (query.VendedorId.HasValue)
                filtradas = filtradas.Where(s => s.VendedorId == query.VendedorId.Value);
            if (query.Estado.HasValue)
                filtradas = filtradas.Where(s => s.Estado == query.Estado.Value);
            if (query.CategoriaId.HasValue)
                filtradas = filtradas.Where(s => s.CategoriaId == query.CategoriaId.Value);
            if (query.CompradorId.HasValue)
                filtradas = filtradas.Where(s => s.Pujas != null && s.Pujas.Any(p => p.CompradorId == query.CompradorId.Value));
            if (!string.IsNullOrWhiteSpace(query.Busqueda))
            {
                var texto = query.Busqueda.Trim().ToLower();
                filtradas = filtradas.Where(s => s.Titulo.ToLower().Contains(texto) || s.Descripcion.ToLower().Contains(texto));
            }

            // Líder y conteo EN MEMORIA sobre las Pujas ya incluidas (0 queries extra)
            var dtos = filtradas.ToList().Select(s =>
            {
                var lider = s.Pujas.OrderByDescending(p => p.Monto).FirstOrDefault();
                return new SubastaDto
                {
                    Id = s.Id,
                    VendedorId = s.VendedorId,
                    VendedorNombre = s.Vendedor?.Nombre ?? string.Empty,
                    CategoriaId = s.CategoriaId,
                    CategoriaNombre = s.Categoria?.Nombre ?? string.Empty,
                    Titulo = s.Titulo,
                    Descripcion = s.Descripcion,
                    UrlImagen = s.UrlImagen,
                    PrecioBase = s.PrecioBase,
                    IncrementoMinimo = s.IncrementoMinimo,
                    CompradorLiderId = lider?.CompradorId,
                    OfertaMasAltaActual = lider?.Monto ?? s.PrecioBase,
                    CantidadOfertas = s.Pujas.Count,
                    FechaInicio = s.FechaInicio,
                    FechaFin = s.FechaFin,
                    Estado = s.Estado,
                    Version = s.Version
                };
            }).ToList();

            var ordenadas = (query.OrdenarPor.ToLower(), query.Orden.ToLower()) switch
            {
                ("oferta", "desc") => dtos.OrderByDescending(d => d.OfertaMasAltaActual).ToList(),
                ("oferta", _) => dtos.OrderBy(d => d.OfertaMasAltaActual).ToList(),
                ("preciobase", "desc") => dtos.OrderByDescending(d => d.PrecioBase).ToList(),
                ("preciobase", _) => dtos.OrderBy(d => d.PrecioBase).ToList(),
                ("fechainicio", "desc") => dtos.OrderByDescending(d => d.FechaInicio).ToList(),
                ("fechainicio", _) => dtos.OrderBy(d => d.FechaInicio).ToList(),
                ("fechafin", "desc") => dtos.OrderByDescending(d => d.FechaFin).ToList(),
                _ => dtos.OrderBy(d => d.FechaFin).ToList()
            };

            var pagina = Math.Max(1, query.Pagina);
            var tamano = Math.Clamp(query.TamanoPagina, 1, 50);
            var total = ordenadas.Count;
            var items = ordenadas.Skip((pagina - 1) * tamano).Take(tamano).ToList();

            return new ResultadoPaginadoDto<SubastaDto>(items, total, pagina, tamano, (int)Math.Ceiling(total / (double)tamano));
        }
    }
}