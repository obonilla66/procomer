# 01 - Análisis del requerimiento

## Caso: Calendarización de Inversores

## 1. Contexto del problema

PROCOMER recibe inversores internacionales que visitan Costa Rica por un período limitado para evaluar oportunidades de inversión. Durante estas visitas, se deben coordinar reuniones con funcionarios públicos, representantes institucionales y aliados estratégicos.

Actualmente, la coordinación se realiza de forma manual, lo que genera problemas como:

* Traslapes de horarios.
* Reuniones programadas durante períodos no disponibles.
* Traslados imposibles entre oficinas.
* Participantes asignados sin compartir idioma con el inversor.
* Dificultad para generar itinerarios formales y trazables.
* Falta de automatización para maximizar el aprovechamiento de la visita.

El sistema requerido debe automatizar la elaboración de agendas viables y optimizadas para inversores extranjeros, respetando restricciones de tiempo, idioma, ubicación y disponibilidad.

---

## 2. Objetivo general

Construir una solución distribuida basada en microservicios que permita mantener catálogos maestros, generar agendas automáticas para inversores y producir un PDF formal con el itinerario de visita.

---

## 3. Objetivos específicos

* Mantener actualizado el catálogo de inversores.
* Mantener participantes, oficinas, disponibilidades y matriz de traslados.
* Generar una agenda viable para un inversor en una fecha específica.
* Filtrar participantes que no compartan idioma con el inversor.
* Respetar jornada laboral de 08:00 a 17:00.
* Excluir el período de almuerzo de 12:00 a 13:00.
* Validar tiempos de traslado entre oficinas.
* Maximizar la cantidad de reuniones logradas.
* Minimizar el tiempo total de traslado ante igualdad de reuniones.
* Permitir anulación lógica de agendas.
* Generar un documento PDF formal en español de Costa Rica.
* Desplegar la solución en Azure Container Apps.
* Publicar cada microservicio como contenedor independiente.
* Persistir la información en Azure SQL Database.

---

## 4. Actores del sistema

### Coordinador

Usuario principal del sistema. Se encarga de:

* Registrar inversores.
* Registrar participantes.
* Registrar oficinas.
* Registrar disponibilidad.
* Mantener la matriz de tiempos de traslado.
* Solicitar la generación automática de agendas.
* Revisar agendas generadas.
* Anular agendas.
* Descargar el PDF del itinerario.

### Inversor

Persona extranjera visitante. No necesariamente interactúa directamente con el sistema, pero es el sujeto principal de la agenda.

### Participante

Funcionario, representante institucional o aliado estratégico que puede reunirse con un inversor.

### Panel evaluador

Usuario externo que validará:

* URL pública del frontend.
* APIs públicas.
* Swagger/OpenAPI.
* PDF generado.
* Funcionamiento general de la solución.
* Arquitectura desplegada en Azure.

---

## 5. Entidades principales

### Investor

Representa al inversor visitante.

Campos principales:

* Id.
* Nombre completo.
* Empresa.
* País.
* Idiomas que maneja.
* Fecha de inicio de visita.
* Fecha de cierre de visita.
* Lugar de hospedaje o punto inicial.
* Indicador de agendas activas.

Reglas:

* Debe tener al menos un idioma.
* La fecha de cierre no puede ser anterior a la fecha de inicio.
* No se puede eliminar si tiene agendas activas.

---

### Participant

Representa a una persona que puede recibir al inversor.

Campos principales:

* Id.
* Nombre completo.
* Cargo o institución.
* Oficina asignada.
* Idiomas que domina.
* Bloques horarios disponibles.
* Estado activo o inactivo.

Reglas:

* Debe tener al menos un idioma.
* Debe tener oficina asignada.
* Si está inactivo, no participa en scheduling.

---

### Office

Representa una ubicación física donde pueden ocurrir reuniones.

Campos principales:

* Id.
* Nombre.
* Dirección física.
* Latitud opcional.
* Longitud opcional.

Reglas:

* No se puede dar de baja una oficina si tiene participantes activos asociados.

---

### TravelTime

Representa el tiempo estimado de traslado entre dos oficinas.

Campos principales:

* Id.
* Oficina origen.
* Oficina destino.
* Minutos de traslado.

Reglas:

* El tiempo no puede ser negativo.
* La matriz debe mantenerse simétrica.
* El traslado A -> B debe ser igual al traslado B -> A.

---

### AvailabilityBlock

Representa una franja horaria disponible de un participante.

Campos principales:

* Id.
* Participante.
* Fecha.
* Hora inicio.
* Hora fin.

Reglas:

* La hora final debe ser mayor que la hora inicial.
* Debe pertenecer a un participante existente.

---

### Agenda

Representa una agenda generada para un inversor.

Campos principales:

* Id.
* Inversor.
* Fecha.
* Estado: Activa o Anulada.
* Reuniones.
* Fecha de creación.
* Total de minutos de traslado.

Reglas:

* No puede estar fuera del rango de visita del inversor.
* Puede anularse mediante eliminación lógica.
* El registro se conserva por trazabilidad.

---

### AgendaMeeting

Representa una reunión dentro de una agenda.

Campos principales:

* Id.
* AgendaId.
* Participante.
* Oficina.
* Hora inicio.
* Hora fin.
* Idioma de la reunión.
* Minutos de traslado desde la reunión anterior.

Reglas:

* Debe compartir idioma entre inversor y participante.
* No puede iniciar antes de las 08:00.
* No puede finalizar después de las 17:00.
* No puede solaparse con otra reunión del mismo participante.
* No puede programarse durante almuerzo.
* Debe respetar tiempo de traslado desde la reunión anterior.

---

## 6. Restricciones principales del algoritmo

El algoritmo de scheduling debe:

1. Validar que la fecha solicitada esté dentro del rango de visita del inversor.
2. Filtrar participantes inactivos.
3. Filtrar participantes que no compartan idioma con el inversor.
4. Revisar disponibilidad de participantes para la fecha solicitada.
5. Respetar jornada laboral de 08:00 a 17:00.
6. Excluir almuerzo de 12:00 a 13:00.
7. Construir posibles horarios de reunión según duración estándar.
8. Validar que dos reuniones consecutivas tengan tiempo suficiente para traslado.
9. Evitar solapes.
10. Evitar asignar dos reuniones solapadas al mismo participante.
11. Maximizar cantidad de reuniones realizadas.
12. Priorizar alcanzar la meta solicitada.
13. Ante igualdad de cantidad de reuniones, minimizar tiempo total de traslado.
14. Retornar mensaje claro si no existe solución viable.

---

## 7. Arquitectura propuesta

La solución se implementará como un conjunto de microservicios desacoplados:

```text
Frontend ASP.NET MVC
        |
        | consume HTTP REST
        |
        +--------------------+
        |                    |
        v                    v
Catalog API              Agenda API
        |                    |
        |                    | consume Catalog API
        |                    | consume PDF API
        v                    v
Azure SQL Database       PDF API
        |
        v
Persistencia relacional
```

Microservicios:

1. Catalog API.
2. Agenda API.
3. PDF API.
4. Frontend ASP.NET MVC.

Cada microservicio backend debe mantener Clean Architecture:

```text
Domain
Application
Infrastructure
Api
```

---

## 8. Responsabilidades por componente

### Frontend ASP.NET MVC

Responsable de la interacción con el coordinador.

Debe incluir como mínimo:

* Pantalla CRUD de inversores.
* Pantalla CRUD de participantes.
* Pantalla CRUD de oficinas.
* Pantalla de generación de agendas.
* Visualización dinámica de datos del inversor seleccionado.
* Listado y detalle de agendas.
* Acción para descargar PDF.

---

### Catalog API

Responsable de datos maestros.

Administra:

* Inversores.
* Participantes.
* Oficinas.
* Disponibilidades.
* Matriz de tiempos de traslado.

Expone endpoints CRUD para que el frontend y Agenda API consulten información.

---

### Agenda API

Responsable del núcleo del sistema.

Administra:

* Solicitud de generación automática.
* Algoritmo de scheduling.
* Persistencia de agendas.
* Consulta de agendas.
* Anulación lógica.
* Orquestación con Catalog API y PDF API.

Debe usar:

* HttpClient tipado para comunicarse con otros microservicios.
* Polly o equivalente para resiliencia.
* EF Core para persistencia.
* Pruebas unitarias del algoritmo.

---

### PDF API

Responsable de generar el documento formal del itinerario.

Recibe datos de agenda y devuelve archivo PDF.

El PDF debe contener:

* Encabezado institucional.
* Nombre del inversor.
* Fecha de jornada.
* Tabla de reuniones.
* Datos de participantes.
* Oficina y dirección.
* Idioma de reunión.
* Traslados entre reuniones.
* Fecha de generación.
* Numeración de página.

---

### Azure

La solución se desplegará usando:

* Azure Container Registry para almacenar imágenes Docker.
* Azure Container Apps para ejecutar frontend y microservicios.
* Azure SQL Database para persistencia.
* Variables de entorno y secretos para configuración.
* Swagger/OpenAPI público en cada API.

---

## 9. Riesgos identificados

### Riesgo 1: Tiempo limitado

La prueba dura seis horas, por lo que se debe priorizar funcionalidad crítica sobre perfección visual.

Mitigación:

* Implementar primero backend y algoritmo.
* Crear UI simple.
* Agregar Docker y despliegue mínimo.
* Documentar claramente limitaciones.

---

### Riesgo 2: Complejidad del algoritmo

El scheduling tiene varias restricciones combinadas.

Mitigación:

* Implementar algoritmo incremental.
* Cubrirlo con pruebas unitarias.
* Usar datos pequeños y controlados.
* Retornar mensajes claros en escenarios fallidos.

---

### Riesgo 3: Comunicación entre microservicios

Agenda API depende de Catalog API y PDF API.

Mitigación:

* Usar HttpClient tipado.
* Configurar URLs por variables de entorno.
* Agregar retry, timeout y circuit breaker con Polly.

---

### Riesgo 4: Despliegue en Azure

El despliegue puede consumir tiempo.

Mitigación:

* Usar Dockerfiles simples.
* Publicar imágenes en Azure Container Registry.
* Crear Container Apps independientes.
* Usar Azure SQL Database.
* Configurar variables de entorno.

---

## 10. Criterios de aceptación

La solución se considera aceptable si:

* Existe frontend público funcional.
* Existen tres microservicios backend desplegados.
* Cada API tiene Swagger público.
* El frontend consume las APIs.
* Se pueden mantener inversores.
* Se pueden mantener participantes y oficinas.
* Se puede generar una agenda viable.
* Se puede anular una agenda.
* Se puede descargar un PDF.
* La información persiste en SQL Server / Azure SQL Database.
* El algoritmo respeta idioma, horarios, almuerzo, disponibilidad y traslados.
* Existen al menos cinco pruebas unitarias del algoritmo.
* La arquitectura está documentada.
* Los casos de uso están documentados.
