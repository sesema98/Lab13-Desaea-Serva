using System.ComponentModel.DataAnnotations;

namespace ObservatoriosApi.Entidades.Dtos;

public class ReservaObservacionDto
{
    public int Id { get; set; }
    public int ObservatorioId { get; set; }
    public string Observatorio { get; set; } = null!;
    public string Visitante { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public DateTime FechaHora { get; set; }
    public int CantidadPersonas { get; set; }
    public string Estado { get; set; } = null!;
}

public class ReservaObservacionCrearDto
{
    [Range(1, int.MaxValue)] public int ObservatorioId { get; set; }
    [Required, StringLength(100, MinimumLength = 3)] public string Visitante { get; set; } = null!;
    [Required, EmailAddress, StringLength(150)] public string Correo { get; set; } = null!;
    [Required] public DateTime FechaHora { get; set; }
    [Range(1, 50)] public int CantidadPersonas { get; set; }
}

public class ReservaObservacionActualizarDto : ReservaObservacionCrearDto
{
    [Required, RegularExpression("Pendiente|Confirmada|Cancelada")] public string Estado { get; set; } = null!;
}
