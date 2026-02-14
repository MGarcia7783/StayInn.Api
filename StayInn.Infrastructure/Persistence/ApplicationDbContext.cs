using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StayInn.Domain.Entities;
using StayInn.Domain.Enums;

namespace StayInn.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Hotel> Hotel => Set<Hotel>();
        public DbSet<AreaEsparcimiento> AreasEsparcimiento => Set<AreaEsparcimiento>();
        public DbSet<Habitacion> Habitaciones => Set<Habitacion>();
        public DbSet<Reservacion> Reservaciones => Set<Reservacion>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            

            // Hotel
            builder.Entity<Hotel>(static entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Id)
                .ValueGeneratedOnAdd();

                entity.Property(h => h.Nombre)
                .IsRequired()
                .HasMaxLength(70);

                entity.Property(h => h.Email)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(h => h.Telefono)
                .IsRequired()
                .HasMaxLength(20);

                entity.Property(h => h.Descripcion).
                IsRequired().
                HasMaxLength(500);

                entity.Property(h => h.Direccion)
                .IsRequired().
                HasMaxLength(250);

                entity.Property(h => h.Latitud)
                .HasPrecision(9, 6)
                .IsRequired();

                entity.Property(h => h.Longitud)
                .HasPrecision(9, 6)
                .IsRequired();
            });


            // AreaEsparcimiento
            builder.Entity<AreaEsparcimiento>(static entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.Descripcion).
                IsRequired().
                HasMaxLength(500);

                entity.Property(e => e.ImagenUrl).
                IsRequired().
                HasMaxLength(500);

                // Relación con el modelo hotel
                entity.HasOne(h => h.Hotel)
                .WithMany(e => e.AreasEsparcimiento)
                .HasForeignKey(e => e.HotelId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            });


            // Habitación
            builder.Entity<Habitacion>(static entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Id)
                .ValueGeneratedOnAdd();

                entity.Property(h => h.Numero)
                .IsRequired();

                entity.Property(h => h.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

                entity.Property(h => h.PrecioNoche)
                .IsRequired()
                .HasColumnType("decimal(11,2)");

                entity.Property(h => h.ImagenUrl)
                .IsRequired()
                .HasMaxLength(500);

                entity.Property(h => h.EstaDisponible)
                .HasDefaultValue(true)
                .IsRequired();

                entity.Property(h => h.CapacidadMax)
                .HasDefaultValue(1)
                .IsRequired();

                // Relación con el modelo hotel
                entity.HasOne(h => h.Hotel)
                .WithMany(x => x.Habitaciones)
                .HasForeignKey(h => h.HotelId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            });


            // Reservacion
            builder.Entity<Reservacion>(static entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id)
                .ValueGeneratedOnAdd();

                entity.Property(r => r.FechaEntrada)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

                entity.Property(r => r.FechaSalida)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

                entity.Property(r => r.MontoTotal)
                .IsRequired()
                .HasColumnType("decimal(11,2)");

                entity.Property(r => r.Estado)
                .HasDefaultValue(EstadoReservacion.Pendiente)
                .IsRequired();

                // Indices para optimizar consultas
                entity.HasIndex(r => r.HabitacionId);
                entity.HasIndex(r => r.UsuarioId);
                entity.HasIndex(r => new { r.FechaEntrada, r.FechaSalida });


                // Relación con el modelo habitación
                entity.HasOne(r => r.Habitacion)
                .WithMany(h => h.Reservaciones)
                .HasForeignKey(r => r.HabitacionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

                // Relación con el modelo usuario
                entity.HasOne(r => r.Usuario)
                .WithMany(u => u.Reservaciones)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            });
        }
    }
}
