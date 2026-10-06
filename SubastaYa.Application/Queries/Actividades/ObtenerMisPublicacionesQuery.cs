using SubastaYa.Application.Abstractions;
using SubastaYa.Application.DTOs.Actividades;

namespace SubastaYa.Application.Queries.Actividades;

public record ObtenerMisPublicacionesQuery(int UsuarioId) : IQuery<IEnumerable<ActividadPublicacionDto>>;