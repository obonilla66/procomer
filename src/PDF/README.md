# PDF Microservice

Microservicio .NET 9 para generar un PDF profesional de una agenda confirmada.

## Objetivo

Dado un `IdAgenda`, el servicio consulta la agenda existente en SQL Server y devuelve un archivo PDF listo para impresión o envío por correo.

## Arquitectura

Se respeta la misma estructura del microservicio de Agenda:

```text
PDF.Api
PDF.Application
PDF.Domain
PDF.Infrastructure
```

## Responsabilidad por capa

### PDF.Api

Contiene la capa REST.

Responsabilidades:

- Exponer endpoint HTTP.
- Configurar Swagger.
- Registrar dependencias.
- Devolver el PDF como `application/pdf`.

Endpoint principal:

```http
GET /api/pdf/agendas/{idAgenda}
```

### PDF.Application

Contiene el caso de uso.

Responsabilidades:

- Validar el parámetro de entrada.
- Solicitar la agenda consolidada mediante interfaces.
- Verificar que la agenda esté confirmada.
- Solicitar la generación del PDF.
- Devolver una respuesta funcional para la API.

No conoce EF Core, SQL Server ni QuestPDF.

### PDF.Domain

Contiene modelos y reglas de negocio.

Responsabilidades:

- Representar la agenda consolidada para PDF.
- Validar reglas como agenda confirmada y con reuniones.

### PDF.Infrastructure

Contiene implementaciones técnicas.

Responsabilidades:

- Consultar SQL Server con EF Core.
- Mapear las tablas existentes.
- Generar el PDF con QuestPDF.

## Base de datos

La base ya existe y se llama:

```text
procomer
```

La cadena de conexión está en:

```text
PDF.Api/appsettings.json
```

No se usan migrations en este microservicio porque el modelo de datos pertenece al microservicio de Agenda.

## Contenido del PDF

El documento generado incluye:

- Encabezado institucional.
- Logo si existe `PDF.Api/Assets/logo.png`.
- Nombre completo del inversor.
- Empresa y país de origen.
- Fecha de la jornada.
- Tabla de reuniones:
  - Hora inicio.
  - Hora fin.
  - Participante.
  - Cargo.
  - Oficina.
  - Dirección física.
  - Idioma.
- Tiempo estimado de traslado hacia la siguiente reunión.
- Pie de página con fecha de generación y número de página.

## Ejecutar

Desde la carpeta donde está `PDF.sln`:

```powershell
dotnet restore
dotnet build
dotnet run --project .\PDF.Api\PDF.Api.csproj
```

Luego abrir:

```text
https://localhost:<puerto>/swagger
```

## Prueba rápida

```powershell
Invoke-WebRequest `
  -Uri "https://localhost:<puerto>/api/pdf/agendas/1" `
  -OutFile "agenda-1.pdf" `
  -SkipCertificateCheck
```

Cambie `<puerto>` por el puerto mostrado por la consola.

## Paquete principal usado

Se usa QuestPDF para la generación del documento.

## Comentarios para programador

Los archivos principales incluyen comentarios `/* ... */` al inicio para explicar su propósito y responsabilidad dentro de la arquitectura.
