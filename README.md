# Laboratorio 13 - API de Observatorios

API REST sencilla para administrar observatorios astronómicos y reservas de visitas. Fue desarrollada con **.NET 10**, Entity Framework Core, DTOs y AutoMapper.

## Requisitos

- .NET SDK 10
- Git
- Visual Studio Code con la extensión **C# Dev Kit**

## Ejecución rápida

Esta opción usa una base de datos temporal y no necesita SQL Server.

```bash
git clone https://github.com/sesema98/Lab13-Desaea-Serva.git
cd Lab13-Desaea-Serva
dotnet restore
dotnet build
UseInMemoryDatabase=true dotnet run --project ObservatoriosApi.Api --urls http://127.0.0.1:5087
```

Con la API ejecutándose, abre en el navegador:

```text
http://127.0.0.1:5087/swagger
```

Para detenerla, presiona `Ctrl + C` en la terminal.

## Ejecutar las pruebas

Deja la API encendida y abre otra terminal en la carpeta del proyecto:

```bash
chmod +x tests/probar-api.sh
./tests/probar-api.sh http://127.0.0.1:5087
```

Las pruebas revisan los CRUD, el filtro, la ruta anidada y las respuestas **200, 201, 204, 400, 404 y 409**.

## Ejecución con SQL Server

1. Ejecuta `database/observatorios.sql` en SQL Server.
2. Guarda tu cadena de conexión local:

```bash
cd ObservatoriosApi.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Observatorios" "TU_CADENA_DE_CONEXION"
cd ..
dotnet run --project ObservatoriosApi.Api
```

La cadena real no debe subirse a GitHub.

## Estructura

- `ObservatoriosApi.Api`: controladores y Swagger.
- `ObservatoriosApi.Negocio`: servicios, reglas y AutoMapper.
- `ObservatoriosApi.Datos`: DbContext y repositorios.
- `ObservatoriosApi.Entidades`: entidades y DTOs.
- `database`: script de la base de datos.
- `tests`: prueba automática de los endpoints.

## Funciones principales

- CRUD de observatorios.
- CRUD de reservas.
- Filtro de reservas por estado y fecha.
- Consulta de reservas de un observatorio.
- Validaciones y reglas de negocio.
