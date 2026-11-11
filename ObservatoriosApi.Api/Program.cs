using Microsoft.EntityFrameworkCore;
using ObservatoriosApi.Datos;
using ObservatoriosApi.Datos.Repositorios;
using ObservatoriosApi.Negocio;
using ObservatoriosApi.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ObservatoriosContext>(options =>
{
    if (builder.Configuration.GetValue<bool>("UseInMemoryDatabase"))
        options.UseInMemoryDatabase("ObservatoriosPruebas");
    else
        options.UseSqlServer(builder.Configuration.GetConnectionString("Observatorios"));
});
builder.Services.AddScoped<IObservatorioRepositorio, ObservatorioRepositorio>();
builder.Services.AddScoped<IReservaObservacionRepositorio, ReservaObservacionRepositorio>();
builder.Services.AddScoped<IObservatorioServicio, ObservatorioServicio>();
builder.Services.AddScoped<IReservaObservacionServicio, ReservaObservacionServicio>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var app = builder.Build();
if (app.Configuration.GetValue<bool>("UseInMemoryDatabase"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ObservatoriosContext>();
    await db.Database.EnsureCreatedAsync();
    if (!await db.Observatorios.AnyAsync())
    {
        db.Observatorios.AddRange(
            new ObservatoriosApi.Entidades.Observatorio { Nombre = "Cielo Andino", Ciudad = "Huaraz", Capacidad = 35, Activo = true },
            new ObservatoriosApi.Entidades.Observatorio { Nombre = "Mirador del Sur", Ciudad = "Arequipa", Capacidad = 25, Activo = true },
            new ObservatoriosApi.Entidades.Observatorio { Nombre = "Horizonte Amazónico", Ciudad = "Tarapoto", Capacidad = 20, Activo = false });
        await db.SaveChangesAsync();
    }
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
