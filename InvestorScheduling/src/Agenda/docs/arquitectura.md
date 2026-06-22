# Arquitectura del microservicio Agenda

## Capas

```text
Agenda.Api
Agenda.Application
Agenda.Domain
Agenda.Infrastructure
```

## Dependencias

```text
Agenda.Api -> Agenda.Application
Agenda.Api -> Agenda.Infrastructure

Agenda.Application -> Agenda.Domain

Agenda.Infrastructure -> Agenda.Application
Agenda.Infrastructure -> Agenda.Domain
```

La API referencia Infrastructure únicamente para registrar dependencias.

## Regla importante

El algoritmo de scheduling vive en Application y depende de interfaces.
Infrastructure implementa esas interfaces usando EF Core.
