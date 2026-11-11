using ObservatoriosApi.Entidades;

namespace ObservatoriosApi.Datos.Repositorios;

public interface IReservaObservacionRepositorio
{
    Task<List<ReservaObservacion>> ListarAsync(string? estado, DateTime? desde, DateTime? hasta, int? observatorioId = null);
    Task<ReservaObservacion?> ObtenerAsync(int id);
    Task<bool> ExisteCruceAsync(int observatorioId, DateTime fechaHora, int? exceptoId = null);
    Task AgregarAsync(ReservaObservacion entidad);
    void Eliminar(ReservaObservacion entidad);
    Task GuardarAsync();
}
