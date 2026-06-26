using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using TicoBus.Model;

namespace TicoBus.DA.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Chofer> Choferes { get; set; }
        public DbSet<Pasajero> Pasajeros { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Unidad> Unidades { get; set; }
        public DbSet<Viaje> Viajes { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chofer>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pasajero>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Viaje>()
                .HasOne(v => v.Chofer)
                .WithMany(c => c.Viajes)
                .HasForeignKey(v => v.ChoferId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Pasajero)
                .WithMany(p => p.Reservas)
                .HasForeignKey(r => r.PasajeroId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Viaje)
                .WithMany(v => v.Reservas)
                .HasForeignKey(r => r.ViajeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Unidad>()
                .HasIndex(u => u.Placa)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Nombre)
                .IsUnique();

            modelBuilder.Entity<Chofer>()
                .HasIndex(c => c.Identificacion)
                .IsUnique();

            modelBuilder.Entity<Pasajero>()
                .HasIndex(p => p.Identificacion)
                .IsUnique();

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "Administrador",
                    Clave = "TicoBus2025*",
                    Correo = "ticobus860@gmail.com",
                    Rol = RolUsuario.Administrador,
                    IntentosFallidos = 0,
                    BloqueadoHasta = null,
                    Activo = true
                }
            );
        }
    }
}
