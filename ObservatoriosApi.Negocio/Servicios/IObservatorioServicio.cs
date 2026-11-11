using ObservatoriosApi.Entidades.Dtos;

namespace ObservatoriosApi.Negocio.Servicios;

public interface IObservatorioServicio
{
    Task<List<ObservatorioDto>> ListarAsync();
    Task<ObservatorioDto?> ObtenerAsync(int id);
    Task<ObservatorioDto> CrearAsync(ObservatorioCrearDto dto);
    Task<bool> ActualizarAsync(int id, ObservatorioActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
