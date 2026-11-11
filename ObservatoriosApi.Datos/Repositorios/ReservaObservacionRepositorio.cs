using Microsoft.EntityFrameworkCore;
using ObservatoriosApi.Entidades;

namespace ObservatoriosApi.Datos.Repositorios;

public class ReservaObservacionRepositorio(ObservatoriosContext context) : IReservaObservacionRepositorio
{
    public Task<List<ReservaObservacion>> ListarAsync(string? estado, DateTime? desde, DateTime? hasta, int? observatorioId = null)
    {
        var q = context.ReservasObservacion.Include(x => x.Observatorio).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(estado)) q = q.Where(x => x.Estado == estado);
        if (desde.HasValue) q = q.Where(x => x.FechaHora >= desde);
        if (hasta.HasValue) q = q.Where(x => x.FechaHora <= hasta);
        if (observatorioId.HasValue) q = q.Where(x => x.ObservatorioId == observatorioId);
        return q.OrderBy(x => x.FechaHora).ToListAsync();
    }
    public Task<ReservaObservacion?> ObtenerAsync(int id) => context.ReservasObservacion.Include(x => x.Observatorio).FirstOrDefaultAsync(x => x.Id == id);
    public Task<bool> ExisteCruceAsync(int observatorioId, DateTime fechaHora, int? exceptoId = null) =>
        context.ReservasObservacion.AnyAsync(x => x.ObservatorioId == observatorioId && x.FechaHora == fechaHora && x.Estado != "Cancelada" && (!exceptoId.HasValue || x.Id != exceptoId));
    public Task AgregarAsync(ReservaObservacion entidad) => context.ReservasObservacion.AddAsync(entidad).AsTask();
    public void Eliminar(ReservaObservacion entidad) => context.ReservasObservacion.Remove(entidad);
    public Task GuardarAsync() => context.SaveChangesAsync();
}
