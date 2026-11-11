# Lab13 Desaea Serva

Web API REST en capas para administrar observatorios astronómicos y reservas de sesiones de observación. Un observatorio puede tener muchas reservas. La API recibe y devuelve únicamente DTOs.

## Proyectos

- `ObservatoriosApi.Entidades`: entidades generadas a partir del modelo de datos y DTOs.
- `ObservatoriosApi.Datos`: `DbContext`, interfaces y repositorios con EF Core y LINQ.
- `ObservatoriosApi.Negocio`: servicios, reglas de negocio y `MappingProfile`.
- `ObservatoriosApi.Api`: controladores, Swagger e inyección de dependencias.

La cadena de referencias es `Api -> Negocio -> Datos -> Entidades`.

## Ejecución en VS Code

1. Instala las extensiones C# Dev Kit y REST Client.
2. Ejecuta `database/observatorios.sql` en SQL Server.
3. Guarda la contraseña sin subirla al repositorio:

   ```bash
   cd ObservatoriosApi.Api
   dotnet user-secrets init
   dotnet user-secrets set 'ConnectionStrings:Observatorios' 'TU_CADENA_LOCAL'
   ```

4. Ejecuta `dotnet restore`, `dotnet build` y `dotnet run --project ObservatoriosApi.Api`.
5. Abre `/swagger` o usa `ObservatoriosApi.Api/ObservatoriosApi.http`.

## Pruebas sin SQL Server

Para revisar toda la API antes de configurar SQL Server:

```bash
UseInMemoryDatabase=true dotnet run --project ObservatoriosApi.Api --urls http://127.0.0.1:5087
./tests/probar-api.sh http://127.0.0.1:5087
```

Este modo existe solamente para pruebas. La configuración normal sigue usando SQL Server y el script Database First.

## Scaffold Database First

Después de crear la base, el modelo se puede regenerar con:

```bash
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold 'TU_CADENA' Microsoft.EntityFrameworkCore.SqlServer \
  --project ObservatoriosApi.Datos --startup-project ObservatoriosApi.Api \
  --output-dir Models --context ObservatoriosContext --no-onconfiguring --force
```

Luego se mueven `Observatorio.cs` y `ReservaObservacion.cs` a `ObservatoriosApi.Entidades` y se ajustan los namespaces, como pide la guía.

## Reglas de negocio

- El nombre del observatorio no se puede repetir.
- Un observatorio con reservas no se puede eliminar.
- Las reservas deben ser futuras, respetar la capacidad y pertenecer a un observatorio activo.
- No se permiten dos reservas no canceladas en el mismo observatorio y horario.

## Sustentación del MappingProfile

| Origen -> Destino | Motivo | Configuración especial |
|---|---|---|
| Observatorio -> ObservatorioDto | Entidad a DTO de lectura para no exponer la entidad | Ninguna, los nombres coinciden |
| ObservatorioCrearDto -> Observatorio | DTO de entrada a entidad nueva | Ninguna |
| ObservatorioActualizarDto -> Observatorio | DTO de entrada sobre una entidad existente | Se usa `Map(origen, destino)` en el servicio |
| ReservaObservacion -> ReservaObservacionDto | Entidad a DTO de lectura | `ForMember` obtiene el nombre del observatorio |
| ReservaObservacionCrearDto -> ReservaObservacion | DTO de entrada a entidad nueva | `ForMember` asigna `Pendiente` como estado inicial |
| ReservaObservacionActualizarDto -> ReservaObservacion | DTO de entrada sobre una entidad existente | Se usa `Map(origen, destino)` en el servicio |

No se usa `ReverseMap()` porque los modelos de lectura y escritura tienen responsabilidades distintas.

## Endpoints

- CRUD de observatorios: `GET/POST /api/observatorios`, `GET/PUT/DELETE /api/observatorios/{id}`.
- CRUD de reservas: `GET/POST /api/reservasobservacion`, `GET/PUT/DELETE /api/reservasobservacion/{id}`.
- Filtros: `GET /api/reservasobservacion?estado=Confirmada&desde=2027-01-01&hasta=2027-12-31`.
- Ruta anidada: `GET /api/observatorios/{id}/reservas`.

Los controladores devuelven 200, 201, 204, 400, 404 y 409 según corresponda.

## Publicar el repositorio

```bash
git init
git add .
git commit -m "API en capas de observatorios"
git branch -M main
git remote add origin URL_DE_TU_REPOSITORIO
git push -u origin main
```

No publiques contraseñas ni cadenas de conexión reales. Usa `dotnet user-secrets` para la clave local.
