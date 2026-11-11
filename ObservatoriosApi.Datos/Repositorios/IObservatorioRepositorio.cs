using ObservatoriosApi.Entidades;

namespace ObservatoriosApi.Datos.Repositorios;

public interface IObservatorioRepositorio
{
    Task<List<Observatorio>> ListarAsync();
    Task<Observatorio?> ObtenerAsync(int id);
    Task<bool> ExisteNombreAsync(string nombre, int? exceptoId = null);
    Task<bool> TieneReservasAsync(int id);
    Task AgregarAsync(Observatorio entidad);
    void Eliminar(Observatorio entidad);
    Task GuardarAsync();
}
