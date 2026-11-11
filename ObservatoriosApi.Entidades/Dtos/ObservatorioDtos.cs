using System.ComponentModel.DataAnnotations;

namespace ObservatoriosApi.Entidades.Dtos;

public class ObservatorioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Ciudad { get; set; } = null!;
    public int Capacidad { get; set; }
    public bool Activo { get; set; }
}

public class ObservatorioCrearDto
{
    [Required, StringLength(120, MinimumLength = 3)] public string Nombre { get; set; } = null!;
    [Required, StringLength(80, MinimumLength = 2)] public string Ciudad { get; set; } = null!;
    [Range(1, 500)] public int Capacidad { get; set; }
    public bool Activo { get; set; } = true;
}

public class ObservatorioActualizarDto : ObservatorioCrearDto { }
