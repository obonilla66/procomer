# Estado de integración aplicado

## Cambios críticos implementados

1. Agenda API deja de consultar Inversor, Participante y MatrizTraslado directamente desde base de datos.
2. Agenda.Infrastructure ahora implementa esas interfaces mediante HttpClient tipado hacia Catalog API.
3. Se agregaron políticas Polly: retry, circuit breaker y timeout.
4. Agenda API expone rutas requeridas:
   - POST /agendas/generar
   - GET /agendas
   - GET /agendas/{id}
   - DELETE /agendas/{id}
   - GET /agendas/{id}/pdf
5. Agenda API orquesta PDF API mediante IPdfClient.
6. Frontend MVC consume Catalog API y Agenda API.
7. Se agregaron Dockerfiles para Catalog, Agenda, PDF y Web.
8. Se agregó docker-compose.yml para ejecución local con SQL Server.
9. Se agregó documentación de despliegue conceptual en Azure Container Apps.

## Contratos usados por Agenda hacia Catalog

- GET /api/inversores/{id}
- GET /api/participantes?ids=1,2,3
- GET /api/participante-horarios?fecha=2026-07-06&idsParticipantes=1,2,3
- GET /api/matriz-traslados

## Contrato usado por Agenda hacia PDF

- GET /api/pdf/agendas/{idAgenda}
