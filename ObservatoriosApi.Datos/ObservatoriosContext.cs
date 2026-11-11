using Microsoft.EntityFrameworkCore;
using ObservatoriosApi.Entidades;

namespace ObservatoriosApi.Datos;

public partial class ObservatoriosContext(DbContextOptions<ObservatoriosContext> options) : DbContext(options)
{
    public virtual DbSet<Observatorio> Observatorios { get; set; }
    public virtual DbSet<ReservaObservacion> ReservasObservacion { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Observatorio>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Nombre).IsUnique();
            entity.Property(e => e.Nombre).HasMaxLength(120);
            entity.Property(e => e.Ciudad).HasMaxLength(80);
        });
        modelBuilder.Entity<ReservaObservacion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Visitante).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.HasOne(e => e.Observatorio).WithMany(o => o.ReservasObservacion)
                .HasForeignKey(e => e.ObservatorioId).OnDelete(DeleteBehavior.Restrict);
        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
