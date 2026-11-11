using AutoMapper;
using ObservatoriosApi.Datos.Repositorios;
using ObservatoriosApi.Entidades;
using ObservatoriosApi.Entidades.Dtos;

namespace ObservatoriosApi.Negocio.Servicios;

public class ReservaObservacionServicio(IReservaObservacionRepositorio repo, IObservatorioRepositorio observatorios, IMapper mapper) : IReservaObservacionServicio
{
    public async Task<List<ReservaObservacionDto>> ListarAsync(string? estado, DateTime? desde, DateTime? hasta, int? observatorioId = null) => mapper.Map<List<ReservaObservacionDto>>(await repo.ListarAsync(estado, desde, hasta, observatorioId));
    public async Task<ReservaObservacionDto?> ObtenerAsync(int id) => mapper.Map<ReservaObservacionDto?>(await repo.ObtenerAsync(id));
    public async Task<ReservaObservacionDto> CrearAsync(ReservaObservacionCrearDto dto)
    {
        await ValidarAsync(dto.ObservatorioId, dto.FechaHora, dto.CantidadPersonas);
        if (await repo.ExisteCruceAsync(dto.ObservatorioId, dto.FechaHora)) throw new ReglaNegocioException("El observatorio ya tiene una reserva en esa fecha y hora.");
        var entidad = mapper.Map<ReservaObservacion>(dto);
        await repo.AgregarAsync(entidad); await repo.GuardarAsync();
        return mapper.Map<ReservaObservacionDto>(await repo.ObtenerAsync(entidad.Id));
    }
    public async Task<bool> ActualizarAsync(int id, ReservaObservacionActualizarDto dto)
    {
        var entidad = await repo.ObtenerAsync(id); if (entidad is null) return false;
        await ValidarAsync(dto.ObservatorioId, dto.FechaHora, dto.CantidadPersonas);
        if (await repo.ExisteCruceAsync(dto.ObservatorioId, dto.FechaHora, id)) throw new ReglaNegocioException("El observatorio ya tiene una reserva en esa fecha y hora.");
        mapper.Map(dto, entidad); await repo.GuardarAsync(); return true;
    }
    public async Task<bool> EliminarAsync(int id)
    {
        var entidad = await repo.ObtenerAsync(id); if (entidad is null) return false;
        repo.Eliminar(entidad); await repo.GuardarAsync(); return true;
    }
    private async Task ValidarAsync(int observatorioId, DateTime fechaHora, int cantidad)
    {
        var observatorio = await observatorios.ObtenerAsync(observatorioId) ?? throw new ReglaNegocioException("El observatorio indicado no existe.");
        if (!observatorio.Activo) throw new ReglaNegocioException("El observatorio está inactivo.");
        if (fechaHora <= DateTime.Now) throw new ReglaNegocioException("La reserva debe programarse para una fecha futura.");
        if (cantidad > observatorio.Capacidad) throw new ReglaNegocioException("La cantidad de personas supera la capacidad del observatorio.");
    }
}
