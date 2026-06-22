# Agenda Microservice

Microservicio .NET 9 para generar agendas optimizadas para inversionistas.

## Base de datos

La base existente se llama:

```text
procomer
```

La cadena de conexión está en:

```text
Agenda.Api/appsettings.json
```

Debe contener:

```text
Initial Catalog=procomer
```

## Arquitectura

Se usan 4 proyectos:

```text
Agenda.Api
Agenda.Application
Agenda.Domain
Agenda.Infrastructure
```

## Responsabilidad por capa

### Agenda.Api

Capa REST.

Responsabilidades:

- Exponer endpoints HTTP.
- Configurar Swagger.
- Registrar dependencias.
- Devolver respuestas HTTP.

Endpoint principal:

```http
POST /api/agendas/generar
```

### Agenda.Application

Contiene el caso de uso de generación de agenda.

Responsabilidades:

- Validar parámetros.
- Obtener inversor, participantes y matriz de traslado usando interfaces.
- Generar slots de reunión.
- Aplicar reglas de scheduling.
- Seleccionar la mejor secuencia.
- Solicitar persistencia de la agenda.

No conoce EF Core ni SQL Server.

### Agenda.Domain

Contiene entidades y reglas puras de negocio.

Reglas incluidas:

- Horario laboral de 08:00 a 17:00.
- Almuerzo bloqueado de 12:00 a 13:00.
- Fecha de agenda dentro del período de visita.
- Reuniones dentro del horario laboral.
- No cruce con almuerzo.

### Agenda.Infrastructure

Contiene implementaciones técnicas:

- `AgendaDbContext`.
- Repositorios EF Core.
- Configuración de tablas.
- Seed data.

## Estrategia de scheduling

El algoritmo sigue este flujo:

1. Validar inversor y fecha.
2. Obtener idiomas del inversor.
3. Obtener participantes candidatos activos.
4. Filtrar candidatos que no comparten idioma.
5. Generar slots posibles dentro del horario laboral.
6. Excluir almuerzo.
7. Consultar tiempos de traslado.
8. Buscar la mejor secuencia con backtracking y poda.
9. Maximizar cantidad de reuniones.
10. Ante empate, minimizar traslado total.
11. Guardar agenda y detalle.

## Ejecutar

Desde la carpeta donde está `Agenda.sln`:

```powershell
dotnet restore
dotnet build
dotnet run --project .\Agenda.Api\Agenda.Api.csproj
```

Luego abrir:

```text
https://localhost:<puerto>/swagger
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

## Migraciones

Si se usan migrations:

```powershell
dotnet ef migrations add InitialCreate `
  --project .\Agenda.Infrastructure\Agenda.Infrastructure.csproj `
  --startup-project .\Agenda.Api\Agenda.Api.csproj `
  --context AgendaDbContext
```

Aplicar:

```powershell
dotnet ef database update `
  --project .\Agenda.Infrastructure\Agenda.Infrastructure.csproj `
  --startup-project .\Agenda.Api\Agenda.Api.csproj `
  --context AgendaDbContext
```

## Comentarios para programador

Los archivos principales tienen comentarios `/* ... */` al inicio, explicando su responsabilidad y cómo encajan dentro de la arquitectura.
