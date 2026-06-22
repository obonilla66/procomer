/*
 * DbContext de lectura para el microservicio PDF.
 *
 * Este microservicio no es dueño del modelo de agenda.
 * Solo consulta las tablas existentes creadas por el microservicio de Agenda:
 *
 * - Agenda
 * - AgendaDetalle
 * - Inversor
 * - Participante
 * - Oficina
 * - Idioma
 *
 * Por eso no se incluyen migraciones aquí.
 */

using Microsoft.EntityFrameworkCore;
using PDF.Infrastructure.Persistence.Entities;

namespace PDF.Infrastructure.Persistence;

public class PdfDbContext : DbContext
{
    public PdfDbContext(DbContextOptions<PdfDbContext> options)
        : base(options)
    {
    }

    public DbSet<AgendaRecord> Agendas => Set<AgendaRecord>();
    public DbSet<AgendaDetalleRecord> AgendaDetalles => Set<AgendaDetalleRecord>();
    public DbSet<InversorRecord> Inversores => Set<InversorRecord>();
    public DbSet<ParticipanteRecord> Participantes => Set<ParticipanteRecord>();
    public DbSet<OficinaRecord> Oficinas => Set<OficinaRecord>();
    public DbSet<IdiomaRecord> Idiomas => Set<IdiomaRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgendaRecord>(e =>
        {
            e.ToTable("Agenda");
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Inversor)
                .WithMany()
                .HasForeignKey(x => x.IdInversor);
        });

        modelBuilder.Entity<AgendaDetalleRecord>(e =>
        {
            e.ToTable("AgendaDetalle");
            e.HasKey(x => x.Id);
            e.Property(x => x.CodigoIdioma).HasColumnType("char(2)");

            e.HasOne(x => x.Agenda)
                .WithMany(x => x.Detalles)
                .HasForeignKey(x => x.IdAgenda);

            e.HasOne(x => x.Participante)
                .WithMany()
                .HasForeignKey(x => x.IdParticipante);

            e.HasOne(x => x.Idioma)
                .WithMany()
                .HasForeignKey(x => x.CodigoIdioma);
        });

        modelBuilder.Entity<InversorRecord>(e =>
        {
            e.ToTable("Inversor");
            e.HasKey(x => x.Id);
            e.Property(x => x.NombreCompleto).HasMaxLength(200);
            e.Property(x => x.EmpresaRepresenta).HasMaxLength(200);
            e.Property(x => x.PaisOrigen).HasMaxLength(100);
            e.Property(x => x.LugarHospedaje).HasMaxLength(300);
        });

        modelBuilder.Entity<ParticipanteRecord>(e =>
        {
            e.ToTable("Participante");
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Oficina)
                .WithMany()
                .HasForeignKey(x => x.IdOficina);
        });

        modelBuilder.Entity<OficinaRecord>(e =>
        {
            e.ToTable("Oficina");
            e.HasKey(x => x.Id);
        });

        modelBuilder.Entity<IdiomaRecord>(e =>
        {
            e.ToTable("Idioma");
            e.HasKey(x => x.CodigoIdioma);
            e.Property(x => x.CodigoIdioma).HasColumnType("char(2)");
        });
    }
}
