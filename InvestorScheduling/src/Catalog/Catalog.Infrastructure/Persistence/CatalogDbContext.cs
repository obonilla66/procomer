using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Idioma> Idiomas => Set<Idioma>();
    public DbSet<Inversor> Inversores => Set<Inversor>();
    public DbSet<InversorIdioma> InversorIdiomas => Set<InversorIdioma>();
    public DbSet<Oficina> Oficinas => Set<Oficina>();
    public DbSet<Participante> Participantes => Set<Participante>();
    public DbSet<ParticipanteIdioma> ParticipanteIdiomas => Set<ParticipanteIdioma>();
    public DbSet<ParticipanteHorario> ParticipanteHorarios => Set<ParticipanteHorario>();
    public DbSet<MatrizTraslado> MatrizTraslados => Set<MatrizTraslado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureIdioma(modelBuilder);
        ConfigureInversor(modelBuilder);
        ConfigureInversorIdioma(modelBuilder);
        ConfigureOficina(modelBuilder);
        ConfigureParticipante(modelBuilder);
        ConfigureParticipanteIdioma(modelBuilder);
        ConfigureParticipanteHorario(modelBuilder);
        ConfigureMatrizTraslado(modelBuilder);
    }

    private static void ConfigureIdioma(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Idioma>(entity =>
        {
            entity.ToTable("Idioma");
            entity.HasKey(x => x.CodigoIdioma).HasName("PK_Idioma");

            entity.Property(x => x.CodigoIdioma)
                .HasColumnName("CodigoIdioma")
                .HasColumnType("char(2)")
                .IsRequired();

            entity.Property(x => x.NombreIdioma)
                .HasColumnName("Idioma")
                .HasMaxLength(100)
                .IsRequired();
        });
    }

    private static void ConfigureInversor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inversor>(entity =>
        {
            entity.ToTable("Inversor");
            entity.HasKey(x => x.Id).HasName("PK_Inversor");

            entity.Property(x => x.Id).HasColumnName("Id");
            entity.Property(x => x.NombreCompleto).HasColumnName("NombreCompleto").HasMaxLength(200).IsRequired();
            entity.Property(x => x.EmpresaRepresenta).HasColumnName("EmpresaRepresenta").HasMaxLength(200);
            entity.Property(x => x.PaisOrigen).HasColumnName("PaisOrigen").HasMaxLength(100).IsRequired();
            entity.Property(x => x.FechaInicioVisita).HasColumnName("FechaInicioVisita").HasColumnType("date").IsRequired();
            entity.Property(x => x.FechaFinVisita).HasColumnName("FechaFinVisita").HasColumnType("date").IsRequired();
            entity.Property(x => x.LugarHospedaje).HasColumnName("LugarHospedaje").HasMaxLength(300).IsRequired();

            entity.HasMany(x => x.Idiomas)
                .WithOne(x => x.Inversor)
                .HasForeignKey(x => x.IdInversor);
        });
    }

    private static void ConfigureInversorIdioma(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InversorIdioma>(entity =>
        {
            entity.ToTable("InversorIdioma");
            entity.HasKey(x => x.Id).HasName("PK_InversorIdioma");

            entity.Property(x => x.Id).HasColumnName("Id");
            entity.Property(x => x.IdInversor).HasColumnName("IdInversor").IsRequired();
            entity.Property(x => x.CodigoIdioma).HasColumnName("CodigoIdioma").HasColumnType("char(2)").IsRequired();

            entity.HasIndex(x => new { x.IdInversor, x.CodigoIdioma })
                .IsUnique()
                .HasDatabaseName("UQ_InversorIdioma");

            entity.HasOne(x => x.Inversor)
                .WithMany(x => x.Idiomas)
                .HasForeignKey(x => x.IdInversor)
                .HasConstraintName("FK_InversorIdioma_Inversor");

            entity.HasOne(x => x.Idioma)
                .WithMany()
                .HasForeignKey(x => x.CodigoIdioma)
                .HasConstraintName("FK_InversorIdioma_Idioma");
        });
    }

    private static void ConfigureOficina(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Oficina>(entity =>
        {
            entity.ToTable("Oficina");
            entity.HasKey(x => x.Id).HasName("PK_Oficina");

            entity.Property(x => x.Id).HasColumnName("Id");
            entity.Property(x => x.Nombre).HasColumnName("Nombre").HasMaxLength(150).IsRequired();
            entity.Property(x => x.Direccion).HasColumnName("Direccion").HasMaxLength(300).IsRequired();
            entity.Property(x => x.Latitud).HasColumnName("Latitud").HasColumnType("decimal(9,6)");
            entity.Property(x => x.Longitud).HasColumnName("Longitud").HasColumnType("decimal(9,6)");

            entity.HasMany(x => x.Participantes)
                .WithOne(x => x.Oficina)
                .HasForeignKey(x => x.IdOficina);
        });
    }

    private static void ConfigureParticipante(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Participante>(entity =>
        {
            entity.ToTable("Participante");
            entity.HasKey(x => x.Id).HasName("PK_Participante");

            entity.Property(x => x.Id).HasColumnName("Id");
            entity.Property(x => x.NombreCompleto).HasColumnName("NombreCompleto").HasMaxLength(200).IsRequired();
            entity.Property(x => x.Cargo).HasColumnName("Cargo").HasMaxLength(150);
            entity.Property(x => x.Institucion).HasColumnName("Institucion").HasMaxLength(200).IsRequired();
            entity.Property(x => x.IdOficina).HasColumnName("IdOficina").IsRequired();
            entity.Property(x => x.Estado).HasColumnName("Estado").HasDefaultValue(true).IsRequired();

            entity.HasOne(x => x.Oficina)
                .WithMany(x => x.Participantes)
                .HasForeignKey(x => x.IdOficina)
                .HasConstraintName("FK_Participante_Oficina");

            entity.HasMany(x => x.Idiomas)
                .WithOne(x => x.Participante)
                .HasForeignKey(x => x.IdParticipante);

            entity.HasMany(x => x.Horarios)
                .WithOne(x => x.Participante)
                .HasForeignKey(x => x.IdParticipante);
        });
    }

    private static void ConfigureParticipanteIdioma(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParticipanteIdioma>(entity =>
        {
            entity.ToTable("ParticipanteIdioma");
            entity.HasKey(x => x.Id).HasName("PK_ParticipanteIdioma");

            entity.Property(x => x.Id).HasColumnName("Id");
            entity.Property(x => x.IdParticipante).HasColumnName("IdParticipante").IsRequired();
            entity.Property(x => x.CodigoIdioma).HasColumnName("CodigoIdioma").HasColumnType("char(2)").IsRequired();

            entity.HasIndex(x => new { x.IdParticipante, x.CodigoIdioma })
                .IsUnique()
                .HasDatabaseName("UQ_ParticipanteIdioma");

            entity.HasOne(x => x.Participante)
                .WithMany(x => x.Idiomas)
                .HasForeignKey(x => x.IdParticipante)
                .HasConstraintName("FK_ParticipanteIdioma_Participante");

            entity.HasOne(x => x.Idioma)
                .WithMany()
                .HasForeignKey(x => x.CodigoIdioma)
                .HasConstraintName("FK_ParticipanteIdioma_Idioma");
        });
    }

    private static void ConfigureParticipanteHorario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParticipanteHorario>(entity =>
        {
            entity.ToTable("ParticipanteHorario");
            entity.HasKey(x => x.Id).HasName("PK_ParticipanteHorario");

            entity.Property(x => x.Id).HasColumnName("Id");
            entity.Property(x => x.IdParticipante).HasColumnName("IdParticipante").IsRequired();
            entity.Property(x => x.FechaHoraInicio).HasColumnName("FechaHoraInicio").HasColumnType("datetime2").IsRequired();
            entity.Property(x => x.FechaHoraFin).HasColumnName("FechaHoraFin").HasColumnType("datetime2").IsRequired();

            entity.Property<int>("Minutos")
                .HasColumnName("Minutos")
                .HasComputedColumnSql("DATEDIFF(MINUTE, FechaHoraInicio, FechaHoraFin)");

            entity.HasOne(x => x.Participante)
                .WithMany(x => x.Horarios)
                .HasForeignKey(x => x.IdParticipante)
                .HasConstraintName("FK_ParticipanteHorario_Participante");
        });
    }

    private static void ConfigureMatrizTraslado(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MatrizTraslado>(entity =>
        {
            entity.ToTable("MatrizTraslado");
            entity.HasKey(x => x.Id).HasName("PK_MatrizTraslado");

            entity.Property(x => x.Id).HasColumnName("Id");
            entity.Property(x => x.IdOficinaOrigen).HasColumnName("IdOficinaOrigen").IsRequired();
            entity.Property(x => x.IdOficinaDestino).HasColumnName("IdOficinaDestino").IsRequired();
            entity.Property(x => x.TiempoMinutos).HasColumnName("TiempoMinutos").IsRequired();

            entity.HasIndex(x => new { x.IdOficinaOrigen, x.IdOficinaDestino })
                .IsUnique()
                .HasDatabaseName("UQ_MatrizTraslado");

            entity.HasOne(x => x.OficinaOrigen)
                .WithMany()
                .HasForeignKey(x => x.IdOficinaOrigen)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_MatrizTraslado_OficinaOrigen");

            entity.HasOne(x => x.OficinaDestino)
                .WithMany()
                .HasForeignKey(x => x.IdOficinaDestino)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_MatrizTraslado_OficinaDestino");
        });
    }
}
