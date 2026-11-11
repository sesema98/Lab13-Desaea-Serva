using Microsoft.AspNetCore.Mvc;
using ObservatoriosApi.Entidades.Dtos;
using ObservatoriosApi.Negocio;
using ObservatoriosApi.Negocio.Servicios;

namespace ObservatoriosApi.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class ReservasObservacionController(IReservaObservacionServicio servicio) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<List<ReservaObservacionDto>>> Get([FromQuery] string? estado, [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta) => Ok(await servicio.ListarAsync(estado, desde, hasta));
    [HttpGet("{id:int}")] public async Task<ActionResult<ReservaObservacionDto>> Get(int id) => await servicio.ObtenerAsync(id) is { } item ? Ok(item) : NotFound();
    [HttpPost] public async Task<ActionResult<ReservaObservacionDto>> Post(ReservaObservacionCrearDto dto) { try { var creado = await servicio.CrearAsync(dto); return CreatedAtAction(nameof(Get), new { id = creado.Id }, creado); } catch (ReglaNegocioException ex) { return Conflict(new ProblemDetails { Detail = ex.Message }); } }
    [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id, ReservaObservacionActualizarDto dto) { try { return await servicio.ActualizarAsync(id, dto) ? NoContent() : NotFound(); } catch (ReglaNegocioException ex) { return Conflict(new ProblemDetails { Detail = ex.Message }); } }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => await servicio.EliminarAsync(id) ? NoContent() : NotFound();
}
