using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Actividades;

namespace SubastaYa.Application.Queries.Actividades;

public record ObtenerMisPujasQuery(int UsuarioId) : IQuery<IEnumerable<ActividadPujaDto>>;