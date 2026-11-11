using ObservatoriosApi.Entidades.Dtos;

namespace ObservatoriosApi.Negocio.Servicios;

public interface IReservaObservacionServicio
{
    Task<List<ReservaObservacionDto>> ListarAsync(string? estado, DateTime? desde, DateTime? hasta, int? observatorioId = null);
    Task<ReservaObservacionDto?> ObtenerAsync(int id);
    Task<ReservaObservacionDto> CrearAsync(ReservaObservacionCrearDto dto);
    Task<bool> ActualizarAsync(int id, ReservaObservacionActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
