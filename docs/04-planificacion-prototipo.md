# 04 - Planificación de la solución y prototipo

## 1. Estrategia general de desarrollo

La solución se implementará como un sistema distribuido compuesto por tres microservicios backend y un frontend ASP.NET MVC.

Componentes principales:

```text
InvestorScheduling
├── Catalog API
├── Agenda API
├── PDF API
└── Frontend ASP.NET MVC
```

La solución se desarrollará priorizando los puntos de mayor peso de la rúbrica:

1. Microservicios y Clean Architecture.
2. Algoritmo de generación de agendas.
3. APIs REST funcionales.
4. SQL Server / Azure SQL Database.
5. Unit tests del algoritmo.
6. Frontend MVC funcional.
7. Docker y Azure Container Apps.

---

## 2. Arquitectura planificada

Cada microservicio backend tendrá separación por capas:

```text
Domain
Application
Infrastructure
Api
```

### Domain

Contiene:

* Entidades del negocio.
* Validaciones de reglas críticas.
* Enums.
* Excepciones de dominio.
* Lógica que no depende de tecnología.

Ejemplos:

* Investor.
* Participant.
* Office.
* Agenda.
* AgendaMeeting.
* TravelTime.
* AvailabilityBlock.

---

### Application

Contiene:

* DTOs.
* Interfaces.
* Casos de uso.
* Servicios de aplicación.
* Contratos de repositorios.
* Contratos de clientes HTTP entre microservicios.

Ejemplos:

* GenerateAgendaRequest.
* AgendaResult.
* IAgendaRepository.
* ICatalogClient.
* GenerateAgendaUseCase.
* SchedulingService.

---

### Infrastructure

Contiene detalles técnicos:

* Entity Framework Core.
* DbContext.
* Repositorios SQL Server.
* Dapper si se requieren consultas directas.
* HttpClient tipado.
* Polly.
* Servicios externos.

---

### Api

Contiene:

* Controllers.
* Swagger/OpenAPI.
* Configuración de DI.
* Program.cs.
* Endpoints REST.

---

## 3. Microservicios planificados

## 3.1 Catalog API

Responsabilidad:

Administrar datos maestros del sistema.

Entidades:

* Investors.
* Participants.
* Offices.
* TravelTimes.
* AvailabilityBlocks.

Endpoints mínimos:

```http
GET    /api/investors
GET    /api/investors/{id}
POST   /api/investors
PUT    /api/investors/{id}
DELETE /api/investors/{id}

GET    /api/participants
GET    /api/participants/{id}
POST   /api/participants
PUT    /api/participants/{id}

GET    /api/offices
GET    /api/offices/{id}
POST   /api/offices
PUT    /api/offices/{id}

GET    /api/travel-times
POST   /api/travel-times

GET    /api/availability
POST   /api/availability
```

Prioridad real durante la prueba:

* Implementar completo Investors.
* Implementar consultas y creación básica de Participants.
* Implementar consultas y creación básica de Offices.
* Usar seed data para TravelTimes y Availability si el tiempo es limitado.

---

## 3.2 Agenda API

Responsabilidad:

Generar, consultar y anular agendas.

Endpoints mínimos requeridos:

```http
POST   /agendas/generar
GET    /agendas
GET    /agendas/{id}
DELETE /agendas/{id}
GET    /agendas/{id}/pdf
```

Responsabilidades internas:

* Consultar inversor en Catalog API.
* Consultar participantes candidatos en Catalog API.
* Consultar disponibilidad en Catalog API.
* Consultar matriz de traslados en Catalog API.
* Ejecutar algoritmo de scheduling.
* Persistir agenda generada.
* Anular agenda lógicamente.
* Solicitar PDF a PDF API.

Este microservicio tendrá la prioridad más alta porque contiene el núcleo técnico del problema.

---

## 3.3 PDF API

Responsabilidad:

Generar el itinerario formal en PDF.

Endpoint mínimo:

```http
POST /pdf/agenda
```

Entrada:

* Datos del inversor.
* Fecha de agenda.
* Reuniones.
* Oficinas.
* Traslados.
* Idioma.
* Fecha de generación.

Salida:

```text
application/pdf
```

---

## 3.4 Frontend ASP.NET MVC

Responsabilidad:

Permitir interacción del coordinador con el sistema.

Pantallas mínimas:

1. Mantenimiento de inversores.
2. Mantenimiento de participantes.
3. Mantenimiento de oficinas.
4. Generación de agenda.
5. Listado de agendas.
6. Detalle de agenda.
7. Descarga PDF.

---

## 4. Prototipo de pantallas

## 4.1 Pantalla: Listado de inversores

```text
+-------------------------------------------------------------+
| PROCOMER - Calendarización de Inversores                    |
+-------------------------------------------------------------+
| Menú: Inversores | Participantes | Oficinas | Agendas       |
+-------------------------------------------------------------+

Inversores

[ Nuevo inversor ]

+----+------------------+-------------+----------+-------------+
| Id | Nombre           | Empresa     | País     | Acciones    |
+----+------------------+-------------+----------+-------------+
| 1  | John Investor    | ABC Corp    | USA      | Editar Ver  |
| 2  | María González   | Global Inc  | España   | Editar Ver  |
+----+------------------+-------------+----------+-------------+
```

Campos visibles:

* Nombre.
* Empresa.
* País.
* Idiomas.
* Fecha inicio.
* Fecha cierre.
* Acciones.

---

## 4.2 Pantalla: Formulario de inversor

```text
+-------------------------------------------------------------+
| Nuevo inversor                                              |
+-------------------------------------------------------------+

Nombre completo:     [____________________________]
Empresa:             [____________________________]
País:                [____________________________]

Idiomas:
[ ] Español
[ ] Inglés

Fecha inicio visita: [____/____/______]
Fecha cierre visita: [____/____/______]

Lugar hospedaje / punto inicial:
[________________________________________________]

[ Guardar ] [ Cancelar ]
```

Validaciones:

* Nombre requerido.
* Al menos un idioma.
* Fecha cierre >= fecha inicio.
* Lugar inicial requerido.

---

## 4.3 Pantalla: Participantes

```text
+-------------------------------------------------------------+
| Participantes                                               |
+-------------------------------------------------------------+

[ Nuevo participante ]

+----+------------------+--------------------+----------+--------+
| Id | Nombre           | Cargo / Institución| Oficina  | Estado |
+----+------------------+--------------------+----------+--------+
| 1  | Ana Rodríguez    | Directora          | Central  | Activo |
| 2  | Carlos Mora      | Representante      | Cámara   | Activo |
+----+------------------+--------------------+----------+--------+
```

Formulario:

```text
Nombre completo:      [__________________________]
Cargo / institución:  [__________________________]
Oficina:              [ Seleccione oficina v ]

Idiomas:
[ ] Español
[ ] Inglés

Estado:
[x] Activo

Disponibilidad:
Fecha:       [____/____/______]
Hora inicio: [__:__]
Hora fin:    [__:__]

[ Agregar bloque ]
[ Guardar ]
```

---

## 4.4 Pantalla: Oficinas

```text
+-------------------------------------------------------------+
| Oficinas                                                    |
+-------------------------------------------------------------+

[ Nueva oficina ]

+----+--------------------+--------------------------+----------+
| Id | Nombre             | Dirección                | Acciones |
+----+--------------------+--------------------------+----------+
| 1  | PROCOMER Central   | San José, Costa Rica     | Editar   |
| 2  | Ministerio         | San José, Costa Rica     | Editar   |
+----+--------------------+--------------------------+----------+
```

Formulario:

```text
Nombre:       [________________________]
Dirección:    [________________________]
Latitud:      [____________] opcional
Longitud:     [____________] opcional

[ Guardar ]
```

---

## 4.5 Pantalla: Generar agenda

```text
+-------------------------------------------------------------+
| Generación de agenda                                        |
+-------------------------------------------------------------+

Inversor:
[ Seleccione inversor v ]

Datos del inversor:
Empresa:             ABC Corp
País:                USA
Idiomas:             Inglés, Español
Ventana de visita:   20/06/2026 - 24/06/2026
Punto inicial:       Hotel San José

Fecha de agenda:
[____/____/______]

Duración por reunión:
[ 60 ] minutos

Meta de reuniones:
[ 4 ]

Participantes candidatos:
[x] Ana Rodríguez - Español / Inglés - PROCOMER Central
[x] Carlos Mora - Español - Cámara de Industrias
[x] John Smith - Inglés - Ministerio

[ Generar agenda ]
```

Comportamiento dinámico:

* Al seleccionar inversor, se cargan automáticamente empresa, país, idiomas y ventana de visita.
* Se permite seleccionar participantes candidatos.
* Se valida fecha antes de enviar.
* Se muestra mensaje claro si Agenda API retorna error.

---

## 4.6 Pantalla: Resultado de agenda

```text
+-------------------------------------------------------------+
| Agenda generada                                             |
+-------------------------------------------------------------+

Inversor: John Investor
Fecha:    22/06/2026
Estado:   Activa

Mensaje:
Agenda generada correctamente alcanzando la meta solicitada.

+-------+-------+----------------+----------+------------+---------+
| Inicio| Fin   | Participante   | Oficina  | Traslado   | Idioma  |
+-------+-------+----------------+----------+------------+---------+
| 08:00 | 09:00 | Ana Rodríguez  | Central  | 0 min      | Inglés  |
| 09:30 | 10:30 | John Smith     | Ministerio| 30 min    | Inglés  |
| 11:00 | 12:00 | Carlos Mora    | Cámara   | 30 min     | Español |
+-------+-------+----------------+----------+------------+---------+

[ Descargar PDF ] [ Anular agenda ] [ Volver ]
```

---

## 4.7 Pantalla: Listado de agendas

```text
+-------------------------------------------------------------+
| Agendas                                                     |
+-------------------------------------------------------------+

Filtros:
Inversor: [__________]
Fecha:    [____/____/______]
Estado:   [Todas v]

[ Buscar ]

+----+---------------+------------+---------+-----------+----------+
| Id | Inversor      | Fecha      | Estado  | Reuniones | Acciones |
+----+---------------+------------+---------+-----------+----------+
| 1  | John Investor | 22/06/2026 | Activa  | 3         | Ver PDF  |
| 2  | María G.      | 23/06/2026 | Anulada | 2         | Ver PDF  |
+----+---------------+------------+---------+-----------+----------+
```

---

## 5. Estrategia de implementación

## Fase 1 - Base de solución

Crear:

* Solución .NET.
* Proyectos por microservicio.
* Proyectos por capa.
* Proyecto MVC.
* Proyecto de tests.

Objetivo:

Tener estructura compilando.

---

## Fase 2 - Catálogo mínimo

Implementar:

* Entidades principales.
* DbContext.
* Migraciones.
* Seed data.
* Endpoints GET y POST principales.

Objetivo:

Tener datos disponibles para Agenda API.

---

## Fase 3 - Algoritmo de Agenda

Implementar:

* DTOs.
* SchedulingService.
* GenerateAgendaUseCase.
* Tests unitarios.

Objetivo:

Tener la lógica crítica funcionando.

---

## Fase 4 - Agenda API

Implementar:

* POST /agendas/generar.
* GET /agendas.
* GET /agendas/{id}.
* DELETE /agendas/{id}.
* GET /agendas/{id}/pdf.

Objetivo:

Tener API REST principal funcional.

---

## Fase 5 - PDF API

Implementar:

* POST /pdf/agenda.
* Generación básica de PDF.

Objetivo:

Poder entregar PDF generado.

---

## Fase 6 - Frontend MVC

Implementar pantallas mínimas:

* Inversores.
* Participantes.
* Oficinas.
* Generación de agenda.
* Listado de agendas.
* Descarga PDF.

Objetivo:

Tener URL pública de frontend funcional.

---

## Fase 7 - Docker y Azure

Implementar:

* Dockerfile por componente.
* Publicación en Azure Container Registry.
* Azure Container Apps.
* Azure SQL Database.
* Variables de entorno.
* Swagger público.

Objetivo:

Cumplir entrega desplegada.

---

## 6. Alcance mínimo viable

Si el tiempo se reduce, el alcance mínimo viable será:

* CRUD o mantenimiento básico de inversores.
* Datos seed para participantes, oficinas, disponibilidad y traslados.
* Generación real de agenda.
* Tests del algoritmo.
* PDF básico.
* Frontend simple pero funcional.
* Despliegue en Azure Container Apps.

---

## 7. Elementos diferibles

Si el tiempo no alcanza, se pueden dejar como simplificación documentada:

* CRUD completo de todas las entidades.
* Edición avanzada de disponibilidad.
* Filtros avanzados de agendas.
* Seguridad con Azure AD.
* Reportes adicionales.
* Estilos visuales avanzados.

---

## 8. Justificación de priorización

Se prioriza Agenda API porque contiene:

* Algoritmo principal.
* Reglas de negocio críticas.
* Mayor complejidad técnica.
* Tests exigidos.
* Mayor impacto en la evaluación.

Se prioriza microservicios y despliegue porque la prueba exige explícitamente:

* Tres microservicios backend.
* Frontend.
* Azure Container Apps.
* Azure SQL Database.
* APIs públicas.

Se prioriza funcionalidad sobre apariencia visual porque una solución funcional, desplegada y defendible puntúa mejor que una solución visualmente atractiva pero incompleta.
