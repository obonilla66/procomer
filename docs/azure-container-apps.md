# Despliegue Azure Container Apps

## Componentes

La solución queda preparada para desplegar cuatro contenedores independientes:

- catalog-api
- agenda-api
- pdf-api
- frontend

Cada imagen debe publicarse en Azure Container Registry y luego desplegarse como Azure Container App.

## Variables de entorno requeridas

### Catalog API

```text
ConnectionStrings__DefaultConnection=<Azure SQL connection string>
```

### Agenda API

```text
ConnectionStrings__AgendaDb=<Azure SQL connection string>
Services__CatalogApi=https://<catalog-api-url>
Services__PdfApi=https://<pdf-api-url>
```

### PDF API

```text
ConnectionStrings__PdfDb=<Azure SQL connection string>
```

### Frontend MVC

```text
Services__CatalogApi=https://<catalog-api-url>
Services__AgendaApi=https://<agenda-api-url>
Services__PdfApi=https://<pdf-api-url>
```

## Comandos conceptuales

```powershell
az acr build --registry <acr> --image catalog-api:v1 --file src/Catalog/Catalog.Api/Dockerfile .
az acr build --registry <acr> --image agenda-api:v1 --file src/Agenda/Agenda.Api/Dockerfile .
az acr build --registry <acr> --image pdf-api:v1 --file src/PDF/PDF.Api/Dockerfile .
az acr build --registry <acr> --image frontend:v1 --file src/Web/InvestorScheduling.Web/Dockerfile .
```

Luego crear cada Container App con su imagen, ingress externo y variables de entorno.

## URLs esperadas para entrega

- Frontend público
- Swagger Catalog API: `/swagger`
- Swagger Agenda API: `/swagger`
- Swagger PDF API: `/swagger`

## Nota

Las connection strings deben configurarse como secretos de Azure Container Apps, no quedar quemadas en appsettings.json.
