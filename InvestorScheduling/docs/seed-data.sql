-- Datos mínimos para probar Catalog API y alimentar Agenda API.
-- Ejecutar después de crear las tablas definidas en docs/base de datos.txt.

IF NOT EXISTS (SELECT 1 FROM Idioma WHERE CodigoIdioma = 'es')
    INSERT INTO Idioma (CodigoIdioma, Idioma) VALUES ('es', N'Español');

IF NOT EXISTS (SELECT 1 FROM Idioma WHERE CodigoIdioma = 'en')
    INSERT INTO Idioma (CodigoIdioma, Idioma) VALUES ('en', N'Inglés');

IF EXISTS (SELECT 1 FROM Oficina WHERE Id IN (1,2,3)) OR NOT EXISTS (SELECT 1 FROM Oficina WHERE Id IN (1,2,3))
    SET IDENTITY_INSERT Oficina ON;

IF NOT EXISTS (SELECT 1 FROM Oficina WHERE Id = 1)
    INSERT INTO Oficina (Id, Nombre, Direccion, Latitud, Longitud)
    VALUES (1, N'PROCOMER Central', N'San José, Costa Rica', NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Oficina WHERE Id = 2)
    INSERT INTO Oficina (Id, Nombre, Direccion, Latitud, Longitud)
    VALUES (2, N'Ministerio de Comercio Exterior', N'San José, Costa Rica', NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Oficina WHERE Id = 3)
    INSERT INTO Oficina (Id, Nombre, Direccion, Latitud, Longitud)
    VALUES (3, N'Cámara de Industrias', N'Heredia, Costa Rica', NULL, NULL);

IF EXISTS (SELECT 1 FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = 'Oficina')
    SET IDENTITY_INSERT Oficina OFF;

IF EXISTS (SELECT 1 FROM Participante WHERE Id IN (1,2,3)) OR NOT EXISTS (SELECT 1 FROM Participante WHERE Id IN (1,2,3))
    SET IDENTITY_INSERT Participante ON;

IF NOT EXISTS (SELECT 1 FROM Participante WHERE Id = 1)
    INSERT INTO Participante (Id, NombreCompleto, Cargo, Institucion, IdOficina, Estado)
    VALUES (1, N'Ana Rodríguez', N'Directora', N'PROCOMER', 1, 1);

IF NOT EXISTS (SELECT 1 FROM Participante WHERE Id = 2)
    INSERT INTO Participante (Id, NombreCompleto, Cargo, Institucion, IdOficina, Estado)
    VALUES (2, N'John Smith', N'Aliado estratégico', N'CINDE', 2, 1);

IF NOT EXISTS (SELECT 1 FROM Participante WHERE Id = 3)
    INSERT INTO Participante (Id, NombreCompleto, Cargo, Institucion, IdOficina, Estado)
    VALUES (3, N'Carlos Mora', N'Representante institucional', N'Cámara de Industrias', 3, 1);

IF EXISTS (SELECT 1 FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = 'Participante')
    SET IDENTITY_INSERT Participante OFF;

IF NOT EXISTS (SELECT 1 FROM ParticipanteIdioma WHERE IdParticipante = 1 AND CodigoIdioma = 'es')
    INSERT INTO ParticipanteIdioma (IdParticipante, CodigoIdioma) VALUES (1, 'es');
IF NOT EXISTS (SELECT 1 FROM ParticipanteIdioma WHERE IdParticipante = 1 AND CodigoIdioma = 'en')
    INSERT INTO ParticipanteIdioma (IdParticipante, CodigoIdioma) VALUES (1, 'en');
IF NOT EXISTS (SELECT 1 FROM ParticipanteIdioma WHERE IdParticipante = 2 AND CodigoIdioma = 'en')
    INSERT INTO ParticipanteIdioma (IdParticipante, CodigoIdioma) VALUES (2, 'en');
IF NOT EXISTS (SELECT 1 FROM ParticipanteIdioma WHERE IdParticipante = 3 AND CodigoIdioma = 'es')
    INSERT INTO ParticipanteIdioma (IdParticipante, CodigoIdioma) VALUES (3, 'es');

IF NOT EXISTS (SELECT 1 FROM ParticipanteHorario WHERE IdParticipante = 1 AND FechaHoraInicio = '2026-06-22T08:00:00')
    INSERT INTO ParticipanteHorario (IdParticipante, FechaHoraInicio, FechaHoraFin)
    VALUES (1, '2026-06-22T08:00:00', '2026-06-22T11:30:00');

IF NOT EXISTS (SELECT 1 FROM ParticipanteHorario WHERE IdParticipante = 2 AND FechaHoraInicio = '2026-06-22T09:00:00')
    INSERT INTO ParticipanteHorario (IdParticipante, FechaHoraInicio, FechaHoraFin)
    VALUES (2, '2026-06-22T09:00:00', '2026-06-22T16:00:00');

IF NOT EXISTS (SELECT 1 FROM ParticipanteHorario WHERE IdParticipante = 3 AND FechaHoraInicio = '2026-06-22T13:00:00')
    INSERT INTO ParticipanteHorario (IdParticipante, FechaHoraInicio, FechaHoraFin)
    VALUES (3, '2026-06-22T13:00:00', '2026-06-22T17:00:00');

IF NOT EXISTS (SELECT 1 FROM MatrizTraslado WHERE IdOficinaOrigen = 1 AND IdOficinaDestino = 2)
    INSERT INTO MatrizTraslado (IdOficinaOrigen, IdOficinaDestino, TiempoMinutos) VALUES (1, 2, 20);
IF NOT EXISTS (SELECT 1 FROM MatrizTraslado WHERE IdOficinaOrigen = 2 AND IdOficinaDestino = 1)
    INSERT INTO MatrizTraslado (IdOficinaOrigen, IdOficinaDestino, TiempoMinutos) VALUES (2, 1, 20);
IF NOT EXISTS (SELECT 1 FROM MatrizTraslado WHERE IdOficinaOrigen = 1 AND IdOficinaDestino = 3)
    INSERT INTO MatrizTraslado (IdOficinaOrigen, IdOficinaDestino, TiempoMinutos) VALUES (1, 3, 35);
IF NOT EXISTS (SELECT 1 FROM MatrizTraslado WHERE IdOficinaOrigen = 3 AND IdOficinaDestino = 1)
    INSERT INTO MatrizTraslado (IdOficinaOrigen, IdOficinaDestino, TiempoMinutos) VALUES (3, 1, 35);
IF NOT EXISTS (SELECT 1 FROM MatrizTraslado WHERE IdOficinaOrigen = 2 AND IdOficinaDestino = 3)
    INSERT INTO MatrizTraslado (IdOficinaOrigen, IdOficinaDestino, TiempoMinutos) VALUES (2, 3, 30);
IF NOT EXISTS (SELECT 1 FROM MatrizTraslado WHERE IdOficinaOrigen = 3 AND IdOficinaDestino = 2)
    INSERT INTO MatrizTraslado (IdOficinaOrigen, IdOficinaDestino, TiempoMinutos) VALUES (3, 2, 30);
