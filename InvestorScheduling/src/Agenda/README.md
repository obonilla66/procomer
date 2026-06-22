# Agenda Microservice

Microservicio .NET 9 para generación optimizada de agendas de inversionistas.

## Arquitectura

Proyectos incluidos:

- `Agenda.Api`: capa REST, Swagger, configuración de DI.
- `Agenda.Application`: casos de uso, DTOs, interfaces y algoritmo de scheduling.
- `Agenda.Domain`: entidades y reglas de negocio.
- `Agenda.Infrastructure`: Entity Framework Core, SQL Server, repositorios y seed data.

La API referencia Infrastructure solo para registrar dependencias en `Program.cs`.
El algoritmo de scheduling no conoce EF Core; depende de interfaces definidas en Application.

## Ejecutar

```powershell
dotnet restore
dotnet build
dotnet run --project Agenda.Api
```

Swagger:

```text
https://localhost:<puerto>/swagger
```

## Migraciones

```powershell
dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialCreate `
  --project Agenda.Infrastructure `
  --startup-project Agenda.Api `
  --context AgendaDbContext

dotnet ef database update `
  --project Agenda.Infrastructure `
  --startup-project Agenda.Api `
  --context AgendaDbContext
```

## Request de prueba

```json
{
  "idInversor": 1,
  "fecha": "2026-07-06",
  "duracionReunionMinutos": 60,
  "cantidadReunionesMeta": 4,
  "idsParticipantes": [1, 2, 3, 4, 5, 6]
}
```

Endpoint:

```http
POST /api/agendas/generar
```

## Nota de seguridad

El archivo `appsettings.json` incluye la cadena de conexión solicitada. Para un repositorio público, se recomienda moverla a user-secrets o variables de ambiente.
