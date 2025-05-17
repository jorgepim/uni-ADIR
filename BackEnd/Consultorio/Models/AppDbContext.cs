using Microsoft.EntityFrameworkCore;

namespace Consultorio.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Especialista> Especialistas { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<SeccionTest> SeccionesTest { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<RespuestaOpcion> RespuestasOpcion { get; set; }
        public DbSet<ResultadoTest> ResultadosTest { get; set; }
        public DbSet<RespuestaPaciente> RespuestasPaciente { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Claves primarias explícitas
            modelBuilder.Entity<Rol>().HasKey(r => r.IdRol);
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Especialista>().HasKey(e => e.IdEspecialista);
            modelBuilder.Entity<Paciente>().HasKey(p => p.IdPaciente);
            modelBuilder.Entity<Test>().HasKey(t => t.IdTest);
            modelBuilder.Entity<SeccionTest>().HasKey(s => s.IdSeccion);
            modelBuilder.Entity<Pregunta>().HasKey(p => p.IdPregunta);
            modelBuilder.Entity<RespuestaOpcion>().HasKey(r => r.IdRespuesta);
            modelBuilder.Entity<ResultadoTest>().HasKey(r => r.IdResultado);
            modelBuilder.Entity<RespuestaPaciente>().HasKey(rp => rp.IdRespuestaPaciente);

            // Relaciones
            modelBuilder.Entity<Rol>()
                .HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Especialista)
                .WithOne(e => e.Usuario)
                .HasForeignKey<Especialista>(e => e.IdUsuario);

            modelBuilder.Entity<Especialista>()
                .HasMany(e => e.Pacientes)
                .WithOne(p => p.Especialista)
                .HasForeignKey(p => p.IdEspecialista);

            modelBuilder.Entity<Especialista>()
                .HasMany(e => e.ResultadosTest)
                .WithOne(r => r.Especialista)
                .HasForeignKey(r => r.IdEspecialista);

            modelBuilder.Entity<Paciente>()
                .HasMany(p => p.ResultadosTest)
                .WithOne(r => r.Paciente)
                .HasForeignKey(r => r.IdPaciente);

            modelBuilder.Entity<Test>()
                .HasMany(t => t.SeccionesTest)
                .WithOne(s => s.Test)
                .HasForeignKey(s => s.IdTest);

            modelBuilder.Entity<Test>()
                .HasMany(t => t.ResultadosTest)
                .WithOne(r => r.Test)
                .HasForeignKey(r => r.IdTest);

            modelBuilder.Entity<SeccionTest>()
                .HasMany(s => s.Preguntas)
                .WithOne(p => p.SeccionTest)
                .HasForeignKey(p => p.IdSeccion);

            modelBuilder.Entity<Pregunta>()
                .HasMany(p => p.RespuestasOpcion)
                .WithOne(ro => ro.Pregunta)
                .HasForeignKey(ro => ro.IdPregunta);

            modelBuilder.Entity<Pregunta>()
                .HasMany(p => p.RespuestasPaciente)
                .WithOne(rp => rp.Pregunta)
                .HasForeignKey(rp => rp.IdPregunta);

            modelBuilder.Entity<ResultadoTest>()
                .HasMany(r => r.RespuestasPaciente)
                .WithOne(rp => rp.ResultadoTest)
                .HasForeignKey(rp => rp.IdResultado);

            modelBuilder.Entity<RespuestaPaciente>()
                .HasOne(rp => rp.RespuestaOpcion)
                .WithMany()
                .HasForeignKey(rp => rp.IdRespuestaOpcion)
                .IsRequired(false);
        }
    }
}
