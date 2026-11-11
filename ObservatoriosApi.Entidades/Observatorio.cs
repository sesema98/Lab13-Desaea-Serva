namespace ObservatoriosApi.Entidades;

public partial class Observatorio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Ciudad { get; set; } = null!;
    public int Capacidad { get; set; }
    public bool Activo { get; set; }
    public virtual ICollection<ReservaObservacion> ReservasObservacion { get; set; } = new List<ReservaObservacion>();
}
