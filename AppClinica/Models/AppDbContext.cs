using Microsoft.EntityFrameworkCore;

namespace AppClinica.Models
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
        public DbSet<ResultadoTest> ResultadosTest { get; set; }
        public DbSet<RespuestaPaciente> RespuestasPaciente { get; set; }
        public DbSet<Consentimiento> Consentimientos { get; set; }
        public DbSet<ComentarioSeccionResultado> ComentariosSeccionResultado { get; set; }
 
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
            modelBuilder.Entity<ResultadoTest>().HasKey(r => r.IdResultado);
            modelBuilder.Entity<RespuestaPaciente>().HasKey(rp => rp.IdRespuestaPaciente);
            modelBuilder.Entity<ComentarioSeccionResultado>().HasKey(c => c.IdComentarioSeccion);

            // Relaciones
            modelBuilder.Entity<Rol>()
                .HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Especialista)
                .WithOne(e => e.Usuario)
                .HasForeignKey<Especialista>(e => e.IdUsuario);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Consentimiento)
                .WithOne(c => c.Usuario)
                .HasForeignKey<Usuario>(u => u.IdConsentimiento)
                .IsRequired(false);

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

            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Consentimiento)
                .WithOne(c => c.Paciente)
                .HasForeignKey<Paciente>(p => p.IdConsentimiento);

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
                .HasMany(p => p.RespuestasPaciente)
                .WithOne(rp => rp.Pregunta)
                .HasForeignKey(rp => rp.IdPregunta);

            modelBuilder.Entity<ResultadoTest>()
                .HasMany(r => r.RespuestasPaciente)
                .WithOne(rp => rp.ResultadoTest)
                .HasForeignKey(rp => rp.IdResultado);

            modelBuilder.Entity<ComentarioSeccionResultado>()
                .HasOne(c => c.ResultadoTest)
                .WithMany(r => r.ComentariosSeccion)
                .HasForeignKey(c => c.IdResultado);

            modelBuilder.Entity<ComentarioSeccionResultado>()
                .HasOne(c => c.SeccionTest)
                .WithMany(s => s.ComentariosSeccion)
                .HasForeignKey(c => c.IdSeccion);

        }
    }
}
