using AutoMapper;
using ObservatoriosApi.Datos.Repositorios;
using ObservatoriosApi.Entidades;
using ObservatoriosApi.Entidades.Dtos;

namespace ObservatoriosApi.Negocio.Servicios;

public class ObservatorioServicio(IObservatorioRepositorio repo, IMapper mapper) : IObservatorioServicio
{
    public async Task<List<ObservatorioDto>> ListarAsync() => mapper.Map<List<ObservatorioDto>>(await repo.ListarAsync());
    public async Task<ObservatorioDto?> ObtenerAsync(int id) => mapper.Map<ObservatorioDto?>(await repo.ObtenerAsync(id));
    public async Task<ObservatorioDto> CrearAsync(ObservatorioCrearDto dto)
    {
        if (await repo.ExisteNombreAsync(dto.Nombre)) throw new ReglaNegocioException("Ya existe un observatorio con ese nombre.");
        var entidad = mapper.Map<Observatorio>(dto);
        await repo.AgregarAsync(entidad); await repo.GuardarAsync();
        return mapper.Map<ObservatorioDto>(entidad);
    }
    public async Task<bool> ActualizarAsync(int id, ObservatorioActualizarDto dto)
    {
        var entidad = await repo.ObtenerAsync(id); if (entidad is null) return false;
        if (await repo.ExisteNombreAsync(dto.Nombre, id)) throw new ReglaNegocioException("Ya existe otro observatorio con ese nombre.");
        mapper.Map(dto, entidad); await repo.GuardarAsync(); return true;
    }
    public async Task<bool> EliminarAsync(int id)
    {
        var entidad = await repo.ObtenerAsync(id); if (entidad is null) return false;
        if (await repo.TieneReservasAsync(id)) throw new ReglaNegocioException("No se puede eliminar un observatorio que tiene reservas.");
        repo.Eliminar(entidad); await repo.GuardarAsync(); return true;
    }
}
