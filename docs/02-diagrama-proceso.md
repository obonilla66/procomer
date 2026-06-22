# 02 - Diagrama del proceso y arquitectura

## 1. Diagrama general de arquitectura

```mermaid
flowchart LR
    User[Coordinador] --> Web[Frontend ASP.NET MVC]

    Web --> CatalogApi[Catalog API]
    Web --> AgendaApi[Agenda API]
    Web --> PdfApi[PDF API]

    AgendaApi --> CatalogApi
    AgendaApi --> PdfApi

    CatalogApi --> Sql[(Azure SQL Database)]
    AgendaApi --> Sql

    PdfApi --> PdfFile[Archivo PDF]

    ACR[Azure Container Registry] --> CatalogApp[Azure Container App - Catalog]
    ACR --> AgendaApp[Azure Container App - Agenda]
    ACR --> PdfApp[Azure Container App - PDF]
    ACR --> WebApp[Azure Container App - Frontend]

    CatalogApp --> Sql
    AgendaApp --> Sql
```

---

## 2. Diagrama de Clean Architecture por microservicio

Cada microservicio backend mantiene la siguiente estructura:

```mermaid
flowchart TD
    Api[Api / Presentation Layer]
    Application[Application Layer]
    Domain[Domain Layer]
    Infrastructure[Infrastructure Layer]
    Database[(SQL Server / Azure SQL)]
    External[Servicios externos / APIs]

    Api --> Application
    Application --> Domain
    Infrastructure --> Application
    Infrastructure --> Domain
    Infrastructure --> Database
    Infrastructure --> External
```

Reglas:

* Domain no depende de ninguna otra capa.
* Application depende de Domain.
* Infrastructure implementa contratos definidos en Application.
* Api expone endpoints REST y llama a Application.
* La base de datos es detalle de Infrastructure.
* HttpClient, EF Core, Dapper, QuestPDF y Polly son detalles técnicos de Infrastructure.

---

## 3. Flujo principal: generación de agenda

```mermaid
sequenceDiagram
    actor C as Coordinador
    participant W as Frontend MVC
    participant A as Agenda API
    participant CAPI as Catalog API
    participant DB as Azure SQL Database
    participant P as PDF API

    C->>W: Selecciona inversor, participantes, fecha, duración y meta
    W->>CAPI: Consulta datos del inversor
    CAPI->>DB: Lee Investor
    DB-->>CAPI: Datos del inversor
    CAPI-->>W: Investor DTO

    W->>A: POST /agendas/generar
    A->>CAPI: Consulta inversor
    CAPI->>DB: Lee Investor
    DB-->>CAPI: Investor
    CAPI-->>A: Investor DTO

    A->>CAPI: Consulta participantes candidatos
    CAPI->>DB: Lee Participants
    DB-->>CAPI: Participants
    CAPI-->>A: Participants DTO

    A->>CAPI: Consulta disponibilidad y traslados
    CAPI->>DB: Lee Availability y TravelTimes
    DB-->>CAPI: Datos
    CAPI-->>A: Datos para scheduling

    A->>A: Ejecuta algoritmo de scheduling
    A->>DB: Guarda Agenda y Meetings
    DB-->>A: Agenda persistida

    A->>P: Solicita PDF
    P-->>A: Archivo PDF

    A-->>W: Resultado de agenda generada
    W-->>C: Muestra agenda y opción de descarga
```

---

## 4. Flujo paso a paso del algoritmo de scheduling

```mermaid
flowchart TD
    Start[Inicio: solicitud de agenda] --> ValidateInvestor[Validar inversor]
    ValidateInvestor --> CheckDate{Fecha dentro de visita?}

    CheckDate -- No --> FailDate[Retornar error: fecha fuera de rango]
    CheckDate -- Sí --> LoadCandidates[Cargar participantes candidatos]

    LoadCandidates --> FilterActive[Filtrar participantes activos]
    FilterActive --> FilterLanguage[Filtrar por idioma compartido]

    FilterLanguage --> HasLanguage{Hay candidatos compatibles?}
    HasLanguage -- No --> FailLanguage[Retornar error: sin idioma compartido]

    HasLanguage -- Sí --> LoadAvailability[Cargar disponibilidad por fecha]
    LoadAvailability --> HasAvailability{Hay disponibilidad?}

    HasAvailability -- No --> FailAvailability[Retornar error: sin disponibilidad]
    HasAvailability -- Sí --> BuildSlots[Construir slots de reunión]

    BuildSlots --> WorkHours[Aplicar jornada 08:00-17:00]
    WorkHours --> ExcludeLunch[Excluir almuerzo 12:00-13:00]
    ExcludeLunch --> LoadTravel[Consultar matriz de traslados]

    LoadTravel --> Search[Buscar combinaciones viables]
    Search --> ValidateTravel[Validar traslados entre reuniones]
    ValidateTravel --> AvoidOverlap[Evitar solapes]
    AvoidOverlap --> Optimize[Maximizar reuniones y minimizar traslados]

    Optimize --> HasSolution{Hay solución?}
    HasSolution -- No --> FailSolution[Retornar causa clara]
    HasSolution -- Sí --> Persist[Guardar agenda y reuniones]

    Persist --> GeneratePdf[Solicitar generación de PDF]
    GeneratePdf --> End[Retornar agenda generada]
```

---

## 5. Flujo de anulación de agenda

```mermaid
flowchart TD
    Start[Coordinador solicita anulación] --> FindAgenda[Buscar agenda por id]
    FindAgenda --> Exists{Existe agenda?}

    Exists -- No --> NotFound[Retornar 404]
    Exists -- Sí --> IsCanceled{Ya está anulada?}

    IsCanceled -- Sí --> ReturnOk[Retornar estado actual]
    IsCanceled -- No --> Cancel[Marcar agenda como Anulada]

    Cancel --> Save[Guardar cambio]
    Save --> KeepPdf[Conservar registro y PDF]
    KeepPdf --> End[Retornar 204 No Content]
```

---

## 6. Flujo de generación de PDF

```mermaid
flowchart TD
    Start[Agenda confirmada] --> SendData[Enviar datos a PDF API]
    SendData --> BuildHeader[Construir encabezado institucional]
    BuildHeader --> BuildTable[Construir tabla de reuniones]
    BuildTable --> AddTravel[Agregar traslados entre reuniones]
    AddTravel --> AddFooter[Agregar fecha y paginación]
    AddFooter --> Generate[Generar archivo PDF]
    Generate --> ReturnPdf[Retornar application/pdf]
```

---

## 7. Comunicación entre microservicios

### Frontend hacia microservicios

```text
Frontend MVC -> Catalog API
Frontend MVC -> Agenda API
Frontend MVC -> PDF API
```

### Agenda API hacia otros servicios

```text
Agenda API -> Catalog API
Agenda API -> PDF API
```

### Persistencia

```text
Catalog API -> Azure SQL Database
Agenda API -> Azure SQL Database
```

### Generación de PDF

```text
PDF API -> Retorna archivo PDF
```

---

## 8. Despliegue en Azure

```mermaid
flowchart LR
    Dev[Equipo de desarrollo] --> Build[Docker build]
    Build --> ACR[Azure Container Registry]

    ACR --> ACAWeb[Container App Frontend]
    ACR --> ACACatalog[Container App Catalog API]
    ACR --> ACAAgenda[Container App Agenda API]
    ACR --> ACAPdf[Container App PDF API]

    ACAWeb --> ACACatalog
    ACAWeb --> ACAAgenda
    ACAWeb --> ACAPdf

    ACAAgenda --> ACACatalog
    ACAAgenda --> ACAPdf

    ACACatalog --> AzureSql[(Azure SQL Database)]
    ACAAgenda --> AzureSql
```

---

## 9. Resumen de componentes Azure

| Componente                      | Uso                                |
| ------------------------------- | ---------------------------------- |
| Azure Container Registry        | Almacenar imágenes Docker          |
| Azure Container Apps            | Ejecutar frontend y microservicios |
| Azure SQL Database              | Persistencia relacional            |
| Secrets / Environment variables | Connection strings y URLs          |
| Swagger público                 | Exploración y verificación de APIs |

---

## 10. Decisiones arquitectónicas

### Decisión 1: Microservicios desacoplados

Se separa Catálogo, Agendas y PDF porque tienen responsabilidades distintas:

* Catálogo administra datos maestros.
* Agendas ejecuta lógica de negocio compleja.
* PDF genera documentos.
* Frontend solo presenta información y consume APIs.

---

### Decisión 2: Clean Architecture

Cada backend se organiza por capas para separar reglas de negocio, casos de uso, detalles técnicos y presentación REST.

---

### Decisión 3: HttpClient tipado y resiliencia

Agenda API consume Catalog API y PDF API mediante HttpClient tipado, configurado con:

* Retry.
* Circuit breaker.
* Timeout.

Esto reduce fallos transitorios entre servicios.

---

### Decisión 4: SQL Server / Azure SQL Database

Se usa SQL Server para persistencia estructurada y trazabilidad.

---

### Decisión 5: PDF API separada

La generación de PDF queda aislada para mantener bajo acoplamiento y permitir cambios futuros en el formato del documento sin afectar el algoritmo de agendas.

---

### Decisión 6: Docker + Azure Container Apps

Cada componente se despliega como contenedor independiente, lo cual facilita escalabilidad, despliegue y separación operativa.
