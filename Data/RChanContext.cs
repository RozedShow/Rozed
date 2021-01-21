using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using Modelos;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Data
{

    public class RChanContext : IdentityDbContext<UsuarioModel, IdentityRole<string>, string>
    {
        private readonly string connectionString;

        public RChanContext(DbContextOptions<RChanContext> options) : base(options)
        {
        }
        public RChanContext(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public RChanContext() {}

        public DbSet<HiloModel> Hilos { get; set; }
        public DbSet<HiloAccionModel> HiloAcciones { get; set; }
        public DbSet<ComentarioModel> Comentarios { get; set; }
        public DbSet<MediaModel> Medias { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<NotificacionModel> Notificaciones{ get; set; }
        public DbSet<DenunciaModel> Denuncias { get; set; }
        public DbSet<Sticky> Stickies { get; set; }
        public DbSet<BaneoModel> Bans { get; set; }
        public DbSet<AccionDeModeracion> AccionesDeModeracion { get; set; }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connectionString ?? "Server=127.0.0.1;Port=5433;Database=RozedBack;User Id=postgres;Password=jejetabien;");
            }
            if (!string.IsNullOrEmpty(connectionString))
            {
            }
            else
            {
                // optionsBuilder.UseNpgsql( "Server=127.0.0.1;Port=5433;Database=RChanTest;User Id=postgres;Password=jejetabien;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HiloModel>()
                .HasIndex(h => h.Bump);

            var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            modelBuilder.Entity<HiloModel>()
                .Property(h => h.Encuesta)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, jsonSettings),
                    v => JsonConvert.DeserializeObject<Encuesta>(v, jsonSettings)
                );

            modelBuilder.Entity<AccionDeModeracion>()
                .HasOne(a => a.Comentario)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AccionDeModeracion>()
                .HasOne(a => a.Hilo)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<AccionDeModeracion>()
                .HasOne(a => a.Denuncia)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AccionDeModeracion>()
                .HasOne(a => a.Ban)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
