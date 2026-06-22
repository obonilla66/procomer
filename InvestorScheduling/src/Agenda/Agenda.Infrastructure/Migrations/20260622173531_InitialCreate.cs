using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Agenda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Idioma",
                columns: table => new
                {
                    CodigoIdioma = table.Column<string>(type: "char(2)", nullable: false),
                    Idioma = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idioma", x => x.CodigoIdioma);
                });

            migrationBuilder.CreateTable(
                name: "Inversor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmpresaRepresenta = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PaisOrigen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaInicioVisita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFinVisita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LugarHospedaje = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inversor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oficina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    Longitud = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oficina", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Agenda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdInversor = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DuracionReunionMinutos = table.Column<int>(type: "int", nullable: false),
                    CantidadReunionesMeta = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agenda_Inversor_IdInversor",
                        column: x => x.IdInversor,
                        principalTable: "Inversor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InversorIdioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdInversor = table.Column<int>(type: "int", nullable: false),
                    CodigoIdioma = table.Column<string>(type: "char(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InversorIdioma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InversorIdioma_Idioma_CodigoIdioma",
                        column: x => x.CodigoIdioma,
                        principalTable: "Idioma",
                        principalColumn: "CodigoIdioma",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InversorIdioma_Inversor_IdInversor",
                        column: x => x.IdInversor,
                        principalTable: "Inversor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatrizTraslado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdOficinaOrigen = table.Column<int>(type: "int", nullable: false),
                    IdOficinaDestino = table.Column<int>(type: "int", nullable: false),
                    TiempoMinutos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatrizTraslado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatrizTraslado_Oficina_IdOficinaDestino",
                        column: x => x.IdOficinaDestino,
                        principalTable: "Oficina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatrizTraslado_Oficina_IdOficinaOrigen",
                        column: x => x.IdOficinaOrigen,
                        principalTable: "Oficina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Institucion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdOficina = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participante_Oficina_IdOficina",
                        column: x => x.IdOficina,
                        principalTable: "Oficina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgendaDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAgenda = table.Column<int>(type: "int", nullable: false),
                    FechaHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdParticipante = table.Column<int>(type: "int", nullable: false),
                    CodigoIdioma = table.Column<string>(type: "char(2)", nullable: false),
                    TiempoTrasladoSiguienteOficinaMinutos = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgendaDetalle_Agenda_IdAgenda",
                        column: x => x.IdAgenda,
                        principalTable: "Agenda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgendaDetalle_Idioma_CodigoIdioma",
                        column: x => x.CodigoIdioma,
                        principalTable: "Idioma",
                        principalColumn: "CodigoIdioma",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AgendaDetalle_Participante_IdParticipante",
                        column: x => x.IdParticipante,
                        principalTable: "Participante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgendaParticipante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAgenda = table.Column<int>(type: "int", nullable: false),
                    IdParticipante = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaParticipante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgendaParticipante_Agenda_IdAgenda",
                        column: x => x.IdAgenda,
                        principalTable: "Agenda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgendaParticipante_Participante_IdParticipante",
                        column: x => x.IdParticipante,
                        principalTable: "Participante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipanteHorario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdParticipante = table.Column<int>(type: "int", nullable: false),
                    FechaHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipanteHorario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipanteHorario_Participante_IdParticipante",
                        column: x => x.IdParticipante,
                        principalTable: "Participante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipanteIdioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdParticipante = table.Column<int>(type: "int", nullable: false),
                    CodigoIdioma = table.Column<string>(type: "char(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipanteIdioma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipanteIdioma_Idioma_CodigoIdioma",
                        column: x => x.CodigoIdioma,
                        principalTable: "Idioma",
                        principalColumn: "CodigoIdioma",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipanteIdioma_Participante_IdParticipante",
                        column: x => x.IdParticipante,
                        principalTable: "Participante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Idioma",
                columns: new[] { "CodigoIdioma", "Idioma" },
                values: new object[,]
                {
                    { "DE", "Alemán" },
                    { "EN", "Inglés" },
                    { "ES", "Español" },
                    { "FR", "Francés" },
                    { "PT", "Portugués" }
                });

            migrationBuilder.InsertData(
                table: "Inversor",
                columns: new[] { "Id", "EmpresaRepresenta", "FechaFinVisita", "FechaInicioVisita", "LugarHospedaje", "NombreCompleto", "PaisOrigen" },
                values: new object[,]
                {
                    { 1, "GreenTech Capital", new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hotel Real InterContinental, Escazú", "John Anderson", "Estados Unidos" },
                    { 2, "Bavaria Manufacturing Group", new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "AC Hotel San José Escazú", "Maria Schmidt", "Alemania" }
                });

            migrationBuilder.InsertData(
                table: "Oficina",
                columns: new[] { "Id", "Direccion", "Latitud", "Longitud", "Nombre" },
                values: new object[,]
                {
                    { 1, "San José, Costa Rica", 9.928100m, -84.090700m, "CINDE San José" },
                    { 2, "Escazú, San José, Costa Rica", 9.918900m, -84.139900m, "PROCOMER Escazú" },
                    { 3, "Coyol, Alajuela, Costa Rica", 9.996900m, -84.257300m, "Zona Franca Coyol" },
                    { 4, "Santa Ana, San José, Costa Rica", 9.932400m, -84.182600m, "Parque Empresarial Forum" },
                    { 5, "La Lima, Cartago, Costa Rica", 9.868600m, -83.940000m, "Zona Franca La Lima" },
                    { 6, "Heredia Centro, Costa Rica", 9.998100m, -84.116500m, "Oficina Municipal de Heredia" }
                });

            migrationBuilder.InsertData(
                table: "InversorIdioma",
                columns: new[] { "Id", "CodigoIdioma", "IdInversor" },
                values: new object[,]
                {
                    { 1, "EN", 1 },
                    { 2, "ES", 1 },
                    { 3, "DE", 2 },
                    { 4, "EN", 2 }
                });

            migrationBuilder.InsertData(
                table: "MatrizTraslado",
                columns: new[] { "Id", "IdOficinaDestino", "IdOficinaOrigen", "TiempoMinutos" },
                values: new object[,]
                {
                    { 1, 2, 1, 25 },
                    { 2, 3, 1, 55 },
                    { 3, 4, 1, 35 },
                    { 4, 5, 1, 45 },
                    { 5, 6, 1, 35 },
                    { 6, 1, 2, 25 },
                    { 7, 3, 2, 45 },
                    { 8, 4, 2, 20 },
                    { 9, 5, 2, 60 },
                    { 10, 6, 2, 40 },
                    { 11, 1, 3, 55 },
                    { 12, 2, 3, 45 },
                    { 13, 4, 3, 40 },
                    { 14, 5, 3, 80 },
                    { 15, 6, 3, 35 },
                    { 16, 1, 4, 35 },
                    { 17, 2, 4, 20 },
                    { 18, 3, 4, 40 },
                    { 19, 5, 4, 70 },
                    { 20, 6, 4, 45 },
                    { 21, 1, 5, 45 },
                    { 22, 2, 5, 60 },
                    { 23, 3, 5, 80 },
                    { 24, 4, 5, 70 },
                    { 25, 6, 5, 55 },
                    { 26, 1, 6, 35 },
                    { 27, 2, 6, 40 },
                    { 28, 3, 6, 35 },
                    { 29, 4, 6, 45 },
                    { 30, 5, 6, 55 }
                });

            migrationBuilder.InsertData(
                table: "Participante",
                columns: new[] { "Id", "Cargo", "Estado", "IdOficina", "Institucion", "NombreCompleto" },
                values: new object[,]
                {
                    { 1, "Director de Atracción de Inversión", true, 1, "CINDE", "Carlos Rodríguez Mora" },
                    { 2, "Gerente de Exportaciones", true, 2, "PROCOMER", "María Fernanda Vargas" },
                    { 3, "Gerente de Operaciones", true, 3, "Zona Franca Coyol", "José Pablo Herrera" },
                    { 4, "Directora Comercial", true, 4, "Parque Empresarial Forum", "Laura Jiménez Solís" },
                    { 5, "Coordinador de Inversión", true, 5, "Zona Franca La Lima", "Andrés Quesada Brenes" },
                    { 6, "Alcaldía de Desarrollo Económico", true, 6, "Municipalidad de Heredia", "Sofía Castro Rojas" }
                });

            migrationBuilder.InsertData(
                table: "ParticipanteHorario",
                columns: new[] { "Id", "FechaHoraFin", "FechaHoraInicio", "IdParticipante" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 6, 11, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 6, 8, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2026, 7, 6, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, new DateTime(2026, 7, 6, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, new DateTime(2026, 7, 6, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 6, 8, 30, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, new DateTime(2026, 7, 6, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 6, 13, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 6, new DateTime(2026, 7, 6, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 6, 8, 0, 0, 0, DateTimeKind.Unspecified), 6 }
                });

            migrationBuilder.InsertData(
                table: "ParticipanteIdioma",
                columns: new[] { "Id", "CodigoIdioma", "IdParticipante" },
                values: new object[,]
                {
                    { 1, "ES", 1 },
                    { 2, "EN", 1 },
                    { 3, "ES", 2 },
                    { 4, "EN", 2 },
                    { 5, "ES", 3 },
                    { 6, "EN", 3 },
                    { 7, "FR", 4 },
                    { 8, "EN", 4 },
                    { 9, "ES", 5 },
                    { 10, "EN", 5 },
                    { 11, "ES", 6 },
                    { 12, "EN", 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agenda_IdInversor",
                table: "Agenda",
                column: "IdInversor");

            migrationBuilder.CreateIndex(
                name: "IX_AgendaDetalle_CodigoIdioma",
                table: "AgendaDetalle",
                column: "CodigoIdioma");

            migrationBuilder.CreateIndex(
                name: "IX_AgendaDetalle_IdAgenda",
                table: "AgendaDetalle",
                column: "IdAgenda");

            migrationBuilder.CreateIndex(
                name: "IX_AgendaDetalle_IdParticipante",
                table: "AgendaDetalle",
                column: "IdParticipante");

            migrationBuilder.CreateIndex(
                name: "IX_AgendaParticipante_IdAgenda_IdParticipante",
                table: "AgendaParticipante",
                columns: new[] { "IdAgenda", "IdParticipante" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgendaParticipante_IdParticipante",
                table: "AgendaParticipante",
                column: "IdParticipante");

            migrationBuilder.CreateIndex(
                name: "IX_InversorIdioma_CodigoIdioma",
                table: "InversorIdioma",
                column: "CodigoIdioma");

            migrationBuilder.CreateIndex(
                name: "IX_InversorIdioma_IdInversor_CodigoIdioma",
                table: "InversorIdioma",
                columns: new[] { "IdInversor", "CodigoIdioma" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatrizTraslado_IdOficinaDestino",
                table: "MatrizTraslado",
                column: "IdOficinaDestino");

            migrationBuilder.CreateIndex(
                name: "IX_MatrizTraslado_IdOficinaOrigen_IdOficinaDestino",
                table: "MatrizTraslado",
                columns: new[] { "IdOficinaOrigen", "IdOficinaDestino" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participante_IdOficina",
                table: "Participante",
                column: "IdOficina");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipanteHorario_IdParticipante",
                table: "ParticipanteHorario",
                column: "IdParticipante");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipanteIdioma_CodigoIdioma",
                table: "ParticipanteIdioma",
                column: "CodigoIdioma");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipanteIdioma_IdParticipante_CodigoIdioma",
                table: "ParticipanteIdioma",
                columns: new[] { "IdParticipante", "CodigoIdioma" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgendaDetalle");

            migrationBuilder.DropTable(
                name: "AgendaParticipante");

            migrationBuilder.DropTable(
                name: "InversorIdioma");

            migrationBuilder.DropTable(
                name: "MatrizTraslado");

            migrationBuilder.DropTable(
                name: "ParticipanteHorario");

            migrationBuilder.DropTable(
                name: "ParticipanteIdioma");

            migrationBuilder.DropTable(
                name: "Agenda");

            migrationBuilder.DropTable(
                name: "Idioma");

            migrationBuilder.DropTable(
                name: "Participante");

            migrationBuilder.DropTable(
                name: "Inversor");

            migrationBuilder.DropTable(
                name: "Oficina");
        }
    }
}
