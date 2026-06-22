using Agenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Persistence;

public class AgendaDbContext : DbContext
{
    public AgendaDbContext(DbContextOptions<AgendaDbContext> options)
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
    public DbSet<AgendaEntity> Agendas => Set<AgendaEntity>();
    public DbSet<AgendaParticipante> AgendaParticipantes => Set<AgendaParticipante>();
    public DbSet<AgendaDetalle> AgendaDetalles => Set<AgendaDetalle>();

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
        ConfigureAgenda(modelBuilder);
        ConfigureAgendaParticipante(modelBuilder);
        ConfigureAgendaDetalle(modelBuilder);

        SeedData(modelBuilder);
    }

    private static void ConfigureIdioma(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<Idioma>();
        e.ToTable("Idioma");
        e.HasKey(x => x.CodigoIdioma);
        e.Property(x => x.CodigoIdioma).HasColumnType("char(2)").IsRequired();
        e.Property(x => x.IdiomaNombre).HasColumnName("Idioma").HasMaxLength(100).IsRequired();
    }

    private static void ConfigureInversor(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<Inversor>();
        e.ToTable("Inversor");
        e.HasKey(x => x.Id);
        e.Property(x => x.NombreCompleto).HasMaxLength(200).IsRequired();
        e.Property(x => x.EmpresaRepresenta).HasMaxLength(200);
        e.Property(x => x.PaisOrigen).HasMaxLength(100).IsRequired();
        e.Property(x => x.LugarHospedaje).HasMaxLength(300).IsRequired();
    }

    private static void ConfigureInversorIdioma(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<InversorIdioma>();
        e.ToTable("InversorIdioma");
        e.HasKey(x => x.Id);
        e.Property(x => x.CodigoIdioma).HasColumnType("char(2)").IsRequired();
        e.HasIndex(x => new { x.IdInversor, x.CodigoIdioma }).IsUnique();
        e.HasOne(x => x.Inversor).WithMany(x => x.Idiomas).HasForeignKey(x => x.IdInversor);
        e.HasOne(x => x.Idioma).WithMany(x => x.InversorIdiomas).HasForeignKey(x => x.CodigoIdioma);
    }

    private static void ConfigureOficina(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<Oficina>();
        e.ToTable("Oficina");
        e.HasKey(x => x.Id);
        e.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
        e.Property(x => x.Direccion).HasMaxLength(300).IsRequired();
        e.Property(x => x.Latitud).HasPrecision(9, 6);
        e.Property(x => x.Longitud).HasPrecision(9, 6);
    }

    private static void ConfigureParticipante(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<Participante>();
        e.ToTable("Participante");
        e.HasKey(x => x.Id);
        e.Property(x => x.NombreCompleto).HasMaxLength(200).IsRequired();
        e.Property(x => x.Cargo).HasMaxLength(150);
        e.Property(x => x.Institucion).HasMaxLength(200).IsRequired();
        e.Property(x => x.Estado).HasDefaultValue(true);
        e.HasOne(x => x.Oficina).WithMany(x => x.Participantes).HasForeignKey(x => x.IdOficina);
    }

    private static void ConfigureParticipanteIdioma(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<ParticipanteIdioma>();
        e.ToTable("ParticipanteIdioma");
        e.HasKey(x => x.Id);
        e.Property(x => x.CodigoIdioma).HasColumnType("char(2)").IsRequired();
        e.HasIndex(x => new { x.IdParticipante, x.CodigoIdioma }).IsUnique();
        e.HasOne(x => x.Participante).WithMany(x => x.Idiomas).HasForeignKey(x => x.IdParticipante);
        e.HasOne(x => x.Idioma).WithMany(x => x.ParticipanteIdiomas).HasForeignKey(x => x.CodigoIdioma);
    }

    private static void ConfigureParticipanteHorario(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<ParticipanteHorario>();
        e.ToTable("ParticipanteHorario");
        e.HasKey(x => x.Id);
        e.HasOne(x => x.Participante).WithMany(x => x.Horarios).HasForeignKey(x => x.IdParticipante);
    }

    private static void ConfigureMatrizTraslado(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<MatrizTraslado>();
        e.ToTable("MatrizTraslado");
        e.HasKey(x => x.Id);
        e.HasIndex(x => new { x.IdOficinaOrigen, x.IdOficinaDestino }).IsUnique();
        e.HasOne(x => x.OficinaOrigen).WithMany().HasForeignKey(x => x.IdOficinaOrigen).OnDelete(DeleteBehavior.Restrict);
        e.HasOne(x => x.OficinaDestino).WithMany().HasForeignKey(x => x.IdOficinaDestino).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureAgenda(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<AgendaEntity>();
        e.ToTable("Agenda");
        e.HasKey(x => x.Id);
        e.Property(x => x.Estado).HasDefaultValue(true);
        e.HasOne(x => x.Inversor).WithMany(x => x.Agendas).HasForeignKey(x => x.IdInversor);
    }

    private static void ConfigureAgendaParticipante(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<AgendaParticipante>();
        e.ToTable("AgendaParticipante");
        e.HasKey(x => x.Id);
        e.HasIndex(x => new { x.IdAgenda, x.IdParticipante }).IsUnique();
        e.HasOne(x => x.Agenda).WithMany(x => x.Participantes).HasForeignKey(x => x.IdAgenda);
        e.HasOne(x => x.Participante).WithMany(x => x.AgendaParticipantes).HasForeignKey(x => x.IdParticipante);
    }

    private static void ConfigureAgendaDetalle(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<AgendaDetalle>();
        e.ToTable("AgendaDetalle");
        e.HasKey(x => x.Id);
        e.Property(x => x.CodigoIdioma).HasColumnType("char(2)").IsRequired();
        e.HasOne(x => x.Agenda).WithMany(x => x.Detalles).HasForeignKey(x => x.IdAgenda);
        e.HasOne(x => x.Participante).WithMany(x => x.AgendaDetalles).HasForeignKey(x => x.IdParticipante);
        e.HasOne(x => x.Idioma).WithMany().HasForeignKey(x => x.CodigoIdioma).OnDelete(DeleteBehavior.Restrict);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Idioma>().HasData(
            new Idioma { CodigoIdioma = "ES", IdiomaNombre = "Español" },
            new Idioma { CodigoIdioma = "EN", IdiomaNombre = "Inglés" },
            new Idioma { CodigoIdioma = "FR", IdiomaNombre = "Francés" },
            new Idioma { CodigoIdioma = "PT", IdiomaNombre = "Portugués" },
            new Idioma { CodigoIdioma = "DE", IdiomaNombre = "Alemán" }
        );

        modelBuilder.Entity<Oficina>().HasData(
            new Oficina { Id = 1, Nombre = "CINDE San José", Direccion = "San José, Costa Rica", Latitud = 9.928100m, Longitud = -84.090700m },
            new Oficina { Id = 2, Nombre = "PROCOMER Escazú", Direccion = "Escazú, San José, Costa Rica", Latitud = 9.918900m, Longitud = -84.139900m },
            new Oficina { Id = 3, Nombre = "Zona Franca Coyol", Direccion = "Coyol, Alajuela, Costa Rica", Latitud = 9.996900m, Longitud = -84.257300m },
            new Oficina { Id = 4, Nombre = "Parque Empresarial Forum", Direccion = "Santa Ana, San José, Costa Rica", Latitud = 9.932400m, Longitud = -84.182600m },
            new Oficina { Id = 5, Nombre = "Zona Franca La Lima", Direccion = "La Lima, Cartago, Costa Rica", Latitud = 9.868600m, Longitud = -83.940000m },
            new Oficina { Id = 6, Nombre = "Oficina Municipal de Heredia", Direccion = "Heredia Centro, Costa Rica", Latitud = 9.998100m, Longitud = -84.116500m }
        );

        modelBuilder.Entity<Inversor>().HasData(
            new Inversor { Id = 1, NombreCompleto = "John Anderson", EmpresaRepresenta = "GreenTech Capital", PaisOrigen = "Estados Unidos", FechaInicioVisita = new DateTime(2026, 7, 6), FechaFinVisita = new DateTime(2026, 7, 10), LugarHospedaje = "Hotel Real InterContinental, Escazú" },
            new Inversor { Id = 2, NombreCompleto = "Maria Schmidt", EmpresaRepresenta = "Bavaria Manufacturing Group", PaisOrigen = "Alemania", FechaInicioVisita = new DateTime(2026, 7, 8), FechaFinVisita = new DateTime(2026, 7, 12), LugarHospedaje = "AC Hotel San José Escazú" }
        );

        modelBuilder.Entity<InversorIdioma>().HasData(
            new InversorIdioma { Id = 1, IdInversor = 1, CodigoIdioma = "EN" },
            new InversorIdioma { Id = 2, IdInversor = 1, CodigoIdioma = "ES" },
            new InversorIdioma { Id = 3, IdInversor = 2, CodigoIdioma = "DE" },
            new InversorIdioma { Id = 4, IdInversor = 2, CodigoIdioma = "EN" }
        );

        modelBuilder.Entity<Participante>().HasData(
            new Participante { Id = 1, NombreCompleto = "Carlos Rodríguez Mora", Cargo = "Director de Atracción de Inversión", Institucion = "CINDE", IdOficina = 1, Estado = true },
            new Participante { Id = 2, NombreCompleto = "María Fernanda Vargas", Cargo = "Gerente de Exportaciones", Institucion = "PROCOMER", IdOficina = 2, Estado = true },
            new Participante { Id = 3, NombreCompleto = "José Pablo Herrera", Cargo = "Gerente de Operaciones", Institucion = "Zona Franca Coyol", IdOficina = 3, Estado = true },
            new Participante { Id = 4, NombreCompleto = "Laura Jiménez Solís", Cargo = "Directora Comercial", Institucion = "Parque Empresarial Forum", IdOficina = 4, Estado = true },
            new Participante { Id = 5, NombreCompleto = "Andrés Quesada Brenes", Cargo = "Coordinador de Inversión", Institucion = "Zona Franca La Lima", IdOficina = 5, Estado = true },
            new Participante { Id = 6, NombreCompleto = "Sofía Castro Rojas", Cargo = "Alcaldía de Desarrollo Económico", Institucion = "Municipalidad de Heredia", IdOficina = 6, Estado = true }
        );

        modelBuilder.Entity<ParticipanteIdioma>().HasData(
            new ParticipanteIdioma { Id = 1, IdParticipante = 1, CodigoIdioma = "ES" },
            new ParticipanteIdioma { Id = 2, IdParticipante = 1, CodigoIdioma = "EN" },
            new ParticipanteIdioma { Id = 3, IdParticipante = 2, CodigoIdioma = "ES" },
            new ParticipanteIdioma { Id = 4, IdParticipante = 2, CodigoIdioma = "EN" },
            new ParticipanteIdioma { Id = 5, IdParticipante = 3, CodigoIdioma = "ES" },
            new ParticipanteIdioma { Id = 6, IdParticipante = 3, CodigoIdioma = "EN" },
            new ParticipanteIdioma { Id = 7, IdParticipante = 4, CodigoIdioma = "FR" },
            new ParticipanteIdioma { Id = 8, IdParticipante = 4, CodigoIdioma = "EN" },
            new ParticipanteIdioma { Id = 9, IdParticipante = 5, CodigoIdioma = "ES" },
            new ParticipanteIdioma { Id = 10, IdParticipante = 5, CodigoIdioma = "EN" },
            new ParticipanteIdioma { Id = 11, IdParticipante = 6, CodigoIdioma = "ES" },
            new ParticipanteIdioma { Id = 12, IdParticipante = 6, CodigoIdioma = "EN" }
        );

        modelBuilder.Entity<ParticipanteHorario>().HasData(
            new ParticipanteHorario { Id = 1, IdParticipante = 1, FechaHoraInicio = new DateTime(2026, 7, 6, 8, 0, 0), FechaHoraFin = new DateTime(2026, 7, 6, 11, 30, 0) },
            new ParticipanteHorario { Id = 2, IdParticipante = 2, FechaHoraInicio = new DateTime(2026, 7, 6, 9, 0, 0), FechaHoraFin = new DateTime(2026, 7, 6, 15, 0, 0) },
            new ParticipanteHorario { Id = 3, IdParticipante = 3, FechaHoraInicio = new DateTime(2026, 7, 6, 10, 0, 0), FechaHoraFin = new DateTime(2026, 7, 6, 17, 0, 0) },
            new ParticipanteHorario { Id = 4, IdParticipante = 4, FechaHoraInicio = new DateTime(2026, 7, 6, 8, 30, 0), FechaHoraFin = new DateTime(2026, 7, 6, 16, 0, 0) },
            new ParticipanteHorario { Id = 5, IdParticipante = 5, FechaHoraInicio = new DateTime(2026, 7, 6, 13, 0, 0), FechaHoraFin = new DateTime(2026, 7, 6, 17, 0, 0) },
            new ParticipanteHorario { Id = 6, IdParticipante = 6, FechaHoraInicio = new DateTime(2026, 7, 6, 8, 0, 0), FechaHoraFin = new DateTime(2026, 7, 6, 12, 0, 0) }
        );

        var traslados = new List<MatrizTraslado>();
        var id = 1;
        void Add(int origen, int destino, int minutos) => traslados.Add(new MatrizTraslado { Id = id++, IdOficinaOrigen = origen, IdOficinaDestino = destino, TiempoMinutos = minutos });

        Add(1, 2, 25); Add(1, 3, 55); Add(1, 4, 35); Add(1, 5, 45); Add(1, 6, 35);
        Add(2, 1, 25); Add(2, 3, 45); Add(2, 4, 20); Add(2, 5, 60); Add(2, 6, 40);
        Add(3, 1, 55); Add(3, 2, 45); Add(3, 4, 40); Add(3, 5, 80); Add(3, 6, 35);
        Add(4, 1, 35); Add(4, 2, 20); Add(4, 3, 40); Add(4, 5, 70); Add(4, 6, 45);
        Add(5, 1, 45); Add(5, 2, 60); Add(5, 3, 80); Add(5, 4, 70); Add(5, 6, 55);
        Add(6, 1, 35); Add(6, 2, 40); Add(6, 3, 35); Add(6, 4, 45); Add(6, 5, 55);

        modelBuilder.Entity<MatrizTraslado>().HasData(traslados);
    }
}
