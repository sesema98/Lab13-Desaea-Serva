# Entrega Semana 13 API en capas con DTOs y AutoMapper

## Tema

El proyecto administra observatorios astronómicos y las reservas de sus sesiones de observación. Resuelve el registro de sedes, controla su capacidad y evita reservas inválidas o repetidas.

Las entidades son `Observatorio` y `ReservaObservacion`. La relación es uno a muchos: un observatorio puede tener muchas reservas y cada reserva pertenece a un solo observatorio mediante `ObservatorioId`.

## Reglas de negocio

1. El nombre de un observatorio no se puede repetir.
2. No se puede eliminar un observatorio que tenga reservas.
3. Una reserva debe programarse para una fecha futura.
4. La cantidad de personas no puede superar la capacidad del observatorio.
5. El observatorio debe estar activo.
6. No se permiten dos reservas no canceladas en el mismo observatorio y horario.

Las reglas lanzan `ReglaNegocioException` y los controladores responden con `409 Conflict`.

## Enlace del repositorio

Pendiente de pegar después de publicar la carpeta en la cuenta personal de GitHub. El proyecto ya incluye `.gitignore` y las instrucciones de publicación en `README.md`.

## Script y solución

El script está en `database/observatorios.sql`. Crea la base `ObservatoriosDb`, las tablas, la relación, restricciones y datos de ejemplo.

La solución contiene cuatro proyectos:

- `ObservatoriosApi.Entidades`
- `ObservatoriosApi.Datos`
- `ObservatoriosApi.Negocio`
- `ObservatoriosApi.Api`

## Sustentación de mapeos

| Mapeo | Origen y destino | Motivo | Configuración especial |
|---|---|---|---|
| Observatorio a ObservatorioDto | Entidad a DTO de lectura | La API devuelve solo campos públicos | Ninguna; los nombres coinciden |
| ObservatorioCrearDto a Observatorio | DTO de entrada a entidad | Crea una entidad para guardarla | Ninguna |
| ObservatorioActualizarDto a Observatorio | DTO de entrada a entidad existente | Actualiza el objeto recuperado por el servicio | `Map(origen, destino)` |
| ReservaObservacion a ReservaObservacionDto | Entidad a DTO de lectura | Devuelve la reserva sin exponer la navegación | `ForMember` toma `Observatorio.Nombre` |
| ReservaObservacionCrearDto a ReservaObservacion | DTO de entrada a entidad | Crea la reserva | `ForMember` asigna `Pendiente` |
| ReservaObservacionActualizarDto a ReservaObservacion | DTO de entrada a entidad existente | Actualiza una reserva recuperada | `Map(origen, destino)` |

No se usa `ReverseMap()` porque los DTOs de lectura y entrada representan contratos diferentes.

## Pruebas

Se probaron los CRUD, el filtro por query string y la ruta anidada. Los resultados están en `evidencias/resultados-pruebas.txt` y `evidencias/resultados-pruebas.png`.

- 200 en consultas correctas, filtros y ruta anidada.
- 201 al crear observatorios y reservas.
- 204 al actualizar o eliminar correctamente.
- 400 cuando el DTO no supera las Data Annotations.
- 404 cuando el recurso no existe.
- 409 cuando se incumple una regla de negocio.

## Observaciones

VS Code permite crear, ejecutar y probar la solución mediante la CLI de .NET. El scaffold real requiere una instancia de SQL Server; por ello el proyecto incluye el modelo compatible, el script y el comando para regenerarlo. El modo en memoria permite comprobar la API sin sustituir la configuración normal de SQL Server.

## Conclusiones

La arquitectura separa las responsabilidades de HTTP, negocio y persistencia. Los DTOs protegen el contrato público y eliminan los ciclos de serialización. AutoMapper centraliza las conversiones, mientras que los servicios concentran las validaciones y reglas reutilizables.
