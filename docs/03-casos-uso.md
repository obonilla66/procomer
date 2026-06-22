# 03 - Casos de uso

## UC-01 - Registrar inversor

### Actor principal

Coordinador.

### Objetivo

Registrar un nuevo inversor internacional que visitará Costa Rica.

### Precondiciones

* El coordinador tiene acceso al sistema.
* El inversor no ha sido registrado previamente.

### Datos requeridos

* Nombre completo.
* Empresa.
* País.
* Idiomas.
* Fecha de inicio de visita.
* Fecha de cierre de visita.
* Lugar de hospedaje o punto inicial.

### Flujo principal

1. El coordinador ingresa a la pantalla de inversores.
2. Selecciona la opción de crear inversor.
3. El sistema muestra el formulario.
4. El coordinador ingresa los datos requeridos.
5. El sistema valida los datos.
6. El sistema guarda el inversor.
7. El sistema muestra mensaje de confirmación.

### Flujos alternos

#### A1 - No se selecciona idioma

1. El sistema detecta que el inversor no tiene idiomas.
2. El sistema rechaza la operación.
3. El sistema muestra mensaje: “El inversor debe tener al menos un idioma.”

#### A2 - Fecha de cierre anterior a fecha de inicio

1. El sistema detecta inconsistencia de fechas.
2. El sistema rechaza la operación.
3. El sistema muestra mensaje: “La fecha de cierre no puede ser anterior a la fecha de inicio.”

### Resultado esperado

Inversor registrado correctamente y disponible para generación de agendas.

---

## UC-02 - Actualizar inversor

### Actor principal

Coordinador.

### Objetivo

Modificar información de un inversor existente.

### Precondiciones

* El inversor existe.
* El inversor no se está modificando de forma que invalide agendas activas.

### Flujo principal

1. El coordinador consulta el listado de inversores.
2. Selecciona un inversor.
3. El sistema muestra los datos actuales.
4. El coordinador modifica los campos necesarios.
5. El sistema valida los datos.
6. El sistema guarda los cambios.
7. El sistema muestra mensaje de confirmación.

### Resultado esperado

Datos del inversor actualizados correctamente.

---

## UC-03 - Eliminar inversor

### Actor principal

Coordinador.

### Objetivo

Eliminar un inversor sin agendas activas.

### Precondiciones

* El inversor existe.
* El inversor no tiene agendas activas asociadas.

### Flujo principal

1. El coordinador selecciona un inversor.
2. El coordinador solicita eliminarlo.
3. El sistema valida si tiene agendas activas.
4. Si no tiene agendas activas, el sistema elimina el inversor.
5. El sistema muestra mensaje de confirmación.

### Flujo alterno

#### A1 - El inversor tiene agendas activas

1. El sistema detecta agendas activas.
2. El sistema rechaza la eliminación.
3. El sistema muestra mensaje: “No se puede eliminar un inversor con agendas activas. Primero debe anular las agendas.”

### Resultado esperado

Inversor eliminado solo si no afecta trazabilidad ni agendas activas.

---

## UC-04 - Registrar participante

### Actor principal

Coordinador.

### Objetivo

Registrar una persona que puede participar en reuniones con inversores.

### Datos requeridos

* Nombre completo.
* Cargo o institución.
* Oficina asignada.
* Idiomas.
* Disponibilidad horaria.
* Estado activo o inactivo.

### Flujo principal

1. El coordinador ingresa a la pantalla de participantes.
2. Selecciona crear participante.
3. Ingresa datos personales.
4. Selecciona oficina.
5. Selecciona idiomas.
6. Registra disponibilidad.
7. El sistema valida los datos.
8. El sistema guarda el participante.

### Flujos alternos

#### A1 - Participante sin idioma

El sistema rechaza la operación.

#### A2 - Participante sin oficina

El sistema rechaza la operación.

### Resultado esperado

Participante disponible para ser considerado por el algoritmo de scheduling.

---

## UC-05 - Registrar oficina

### Actor principal

Coordinador.

### Objetivo

Registrar una ubicación física donde pueden realizarse reuniones.

### Datos requeridos

* Nombre de oficina.
* Dirección física.
* Coordenadas opcionales.

### Flujo principal

1. El coordinador ingresa a la pantalla de oficinas.
2. Selecciona crear oficina.
3. Ingresa nombre y dirección.
4. Opcionalmente ingresa coordenadas.
5. El sistema valida los datos.
6. El sistema guarda la oficina.

### Resultado esperado

Oficina disponible para participantes y matriz de traslados.

---

## UC-06 - Mantener matriz de traslados

### Actor principal

Coordinador.

### Objetivo

Registrar tiempos estimados de traslado entre oficinas.

### Datos requeridos

* Oficina origen.
* Oficina destino.
* Tiempo en minutos.

### Flujo principal

1. El coordinador ingresa a matriz de traslados.
2. Selecciona oficina origen.
3. Selecciona oficina destino.
4. Ingresa tiempo estimado.
5. El sistema valida que el tiempo no sea negativo.
6. El sistema guarda el traslado.
7. El sistema crea o actualiza el traslado inverso para mantener simetría.

### Flujo alterno

#### A1 - Tiempo negativo

El sistema rechaza la operación.

### Resultado esperado

La matriz de traslados queda actualizada y simétrica.

---

## UC-07 - Generar agenda automática

### Actor principal

Coordinador.

### Objetivo

Generar una agenda viable y optimizada para un inversor.

### Precondiciones

* El inversor existe.
* Existen participantes candidatos.
* Existen oficinas.
* Existe matriz de traslados.
* Existen disponibilidades para la fecha solicitada.

### Datos requeridos

* Inversor.
* Participantes candidatos.
* Fecha de agenda.
* Duración estándar de reunión.
* Cantidad meta de reuniones.

### Flujo principal

1. El coordinador abre la pantalla de generación de agenda.
2. Selecciona un inversor.
3. El frontend muestra automáticamente empresa, país, idiomas y ventana de visita.
4. El coordinador selecciona participantes candidatos.
5. El coordinador selecciona la fecha de agenda.
6. El coordinador indica duración estándar de reunión.
7. El coordinador indica cantidad meta de reuniones.
8. El frontend envía solicitud a Agenda API.
9. Agenda API consulta datos del inversor en Catalog API.
10. Agenda API consulta participantes, disponibilidad y traslados en Catalog API.
11. El algoritmo valida fecha dentro del rango de visita.
12. El algoritmo filtra participantes sin idioma compartido.
13. El algoritmo revisa disponibilidad dentro de 08:00 a 17:00.
14. El algoritmo excluye 12:00 a 13:00.
15. El algoritmo valida tiempos de traslado.
16. El algoritmo evita solapes.
17. El algoritmo selecciona la mejor combinación.
18. Agenda API guarda agenda y reuniones.
19. Agenda API solicita PDF a PDF API.
20. El frontend muestra la agenda generada.

### Flujos alternos

#### A1 - Fecha fuera del rango de visita

El sistema rechaza la generación y muestra mensaje explicativo.

#### A2 - Ningún participante comparte idioma

El sistema rechaza la generación y muestra mensaje explicativo.

#### A3 - No hay disponibilidad

El sistema rechaza la generación y muestra mensaje explicativo.

#### A4 - No existe combinación viable por traslados

El sistema rechaza o genera agenda parcial y muestra causa.

#### A5 - No se alcanza la meta de reuniones

El sistema puede devolver agenda parcial indicando que no fue posible alcanzar la meta.

### Resultado esperado

Agenda generada con la mayor cantidad posible de reuniones y menor tiempo de traslado ante empate.

---

## UC-08 - Consultar agendas

### Actor principal

Coordinador.

### Objetivo

Listar agendas existentes.

### Filtros opcionales

* Inversor.
* Fecha.
* Estado.

### Flujo principal

1. El coordinador ingresa al listado de agendas.
2. El sistema muestra agendas existentes.
3. El coordinador puede aplicar filtros.
4. El sistema muestra resultados filtrados.

### Resultado esperado

Listado de agendas disponible para seguimiento y control.

---

## UC-09 - Consultar detalle de agenda

### Actor principal

Coordinador.

### Objetivo

Ver detalle completo de una agenda.

### Flujo principal

1. El coordinador selecciona una agenda.
2. El sistema consulta detalle.
3. El sistema muestra:

   * Inversor.
   * Fecha.
   * Estado.
   * Reuniones.
   * Participantes.
   * Oficinas.
   * Idioma.
   * Traslados.
   * Opción de descargar PDF.

### Resultado esperado

Detalle completo visible para validación.

---

## UC-10 - Anular agenda

### Actor principal

Coordinador.

### Objetivo

Anular una agenda conservando trazabilidad.

### Precondiciones

* La agenda existe.
* La agenda no está anulada previamente.

### Flujo principal

1. El coordinador selecciona una agenda.
2. El coordinador solicita anulación.
3. El sistema confirma la acción.
4. El sistema marca la agenda como Anulada.
5. El sistema conserva el registro y PDF.
6. El sistema muestra confirmación.

### Resultado esperado

Agenda anulada lógicamente, sin eliminación física.

---

## UC-11 - Descargar PDF de agenda

### Actor principal

Coordinador.

### Objetivo

Descargar el itinerario formal de una agenda.

### Precondiciones

* La agenda existe.
* Existe información de reuniones.
* PDF API está disponible.

### Flujo principal

1. El coordinador abre el detalle de agenda.
2. Selecciona Descargar PDF.
3. El frontend solicita PDF a Agenda API.
4. Agenda API obtiene o solicita generación a PDF API.
5. PDF API genera documento.
6. El sistema retorna archivo PDF.
7. El navegador descarga o muestra el PDF.

### Resultado esperado

PDF formal generado en español de Costa Rica.

---

## UC-12 - Explorar APIs mediante Swagger

### Actor principal

Panel evaluador.

### Objetivo

Verificar endpoints públicos de cada microservicio.

### Flujo principal

1. El evaluador abre Swagger de Catalog API.
2. Verifica endpoints CRUD.
3. El evaluador abre Swagger de Agenda API.
4. Prueba generación y consulta de agendas.
5. El evaluador abre Swagger de PDF API.
6. Prueba generación o descarga de PDF.

### Resultado esperado

APIs explorables públicamente y funcionales.

---

## Priorización de casos de uso

### Prioridad alta

* UC-01 Registrar inversor.
* UC-04 Registrar participante.
* UC-05 Registrar oficina.
* UC-06 Mantener matriz de traslados.
* UC-07 Generar agenda automática.
* UC-08 Consultar agendas.
* UC-10 Anular agenda.
* UC-11 Descargar PDF.

### Prioridad media

* UC-02 Actualizar inversor.
* UC-03 Eliminar inversor.
* UC-09 Consultar detalle.
* UC-12 Explorar APIs.

### Prioridad baja si el tiempo es limitado

* Mejoras visuales avanzadas.
* Filtros avanzados.
* Reportes adicionales.
* Seguridad avanzada.
* Auditoría detallada.
