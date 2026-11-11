using Microsoft.AspNetCore.Mvc;
using ObservatoriosApi.Entidades.Dtos;
using ObservatoriosApi.Negocio;
using ObservatoriosApi.Negocio.Servicios;

namespace ObservatoriosApi.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class ObservatoriosController(IObservatorioServicio servicio, IReservaObservacionServicio reservas) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<List<ObservatorioDto>>> Get() => Ok(await servicio.ListarAsync());
    [HttpGet("{id:int}")] public async Task<ActionResult<ObservatorioDto>> Get(int id) => await servicio.ObtenerAsync(id) is { } item ? Ok(item) : NotFound();
    [HttpGet("{id:int}/reservas")] public async Task<ActionResult<List<ReservaObservacionDto>>> GetReservas(int id) => await servicio.ObtenerAsync(id) is null ? NotFound() : Ok(await reservas.ListarAsync(null, null, null, id));
    [HttpPost] public async Task<ActionResult<ObservatorioDto>> Post(ObservatorioCrearDto dto) { try { var creado = await servicio.CrearAsync(dto); return CreatedAtAction(nameof(Get), new { id = creado.Id }, creado); } catch (ReglaNegocioException ex) { return Conflict(new ProblemDetails { Detail = ex.Message }); } }
    [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id, ObservatorioActualizarDto dto) { try { return await servicio.ActualizarAsync(id, dto) ? NoContent() : NotFound(); } catch (ReglaNegocioException ex) { return Conflict(new ProblemDetails { Detail = ex.Message }); } }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { try { return await servicio.EliminarAsync(id) ? NoContent() : NotFound(); } catch (ReglaNegocioException ex) { return Conflict(new ProblemDetails { Detail = ex.Message }); } }
}
