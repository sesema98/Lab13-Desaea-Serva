namespace ObservatoriosApi.Entidades;

public partial class ReservaObservacion
{
    public int Id { get; set; }
    public int ObservatorioId { get; set; }
    public string Visitante { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public DateTime FechaHora { get; set; }
    public int CantidadPersonas { get; set; }
    public string Estado { get; set; } = null!;
    public virtual Observatorio Observatorio { get; set; } = null!;
}
