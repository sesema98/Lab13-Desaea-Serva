using Microsoft.EntityFrameworkCore;
using ObservatoriosApi.Entidades;

namespace ObservatoriosApi.Datos.Repositorios;

public class ObservatorioRepositorio(ObservatoriosContext context) : IObservatorioRepositorio
{
    public Task<List<Observatorio>> ListarAsync() => context.Observatorios.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync();
    public Task<Observatorio?> ObtenerAsync(int id) => context.Observatorios.FirstOrDefaultAsync(x => x.Id == id);
    public Task<bool> ExisteNombreAsync(string nombre, int? exceptoId = null) => context.Observatorios.AnyAsync(x => x.Nombre == nombre && (!exceptoId.HasValue || x.Id != exceptoId));
    public Task<bool> TieneReservasAsync(int id) => context.ReservasObservacion.AnyAsync(x => x.ObservatorioId == id);
    public Task AgregarAsync(Observatorio entidad) => context.Observatorios.AddAsync(entidad).AsTask();
    public void Eliminar(Observatorio entidad) => context.Observatorios.Remove(entidad);
    public Task GuardarAsync() => context.SaveChangesAsync();
}
