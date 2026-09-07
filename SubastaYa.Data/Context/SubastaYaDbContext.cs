using Microsoft.EntityFrameworkCore;
using SubastaYa.Core.Domain.Entities;
using SubastaYa.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Data.Context
{
    public class SubastaYaDbContext : DbContext
    {
        // El constructor recibe la configuración desde la API (como la contraseña de MySQL)
        // y se la pasa a la clase base de Entity Framework.
        public SubastaYaDbContext(DbContextOptions<SubastaYaDbContext> options)
            : base(options){
        }

        // Los DbSet le dicen a EF Core cuáles clases deben convertirse en tablas en MySQL.
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Billetera> Billeteras { get; set; }
        public DbSet<Subasta> Subastas { get; set; }
        public DbSet<Puja> Pujas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TransaccionLedger> TransaccionesLedger { get; set; }
        public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }


        // Acá podés forzar configuraciones manuales si las convenciones automáticas no alcanzan
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación 1 a 1 
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Billetera)
                .WithOne(b => b.Usuario)
                .HasForeignKey<Billetera>(b => b.UsuarioId);

            // 1. Optimistic Locking (Requisito estricto del TP)
            modelBuilder.Entity<Subasta>()
                .Property(s => s.Version)
                .IsConcurrencyToken();

            modelBuilder.Entity<Billetera>()
                .Property(b => b.Version)
                .IsConcurrencyToken();

            // 2. Precisión decimal para todos los montos económicos
            modelBuilder.Entity<Billetera>().Property(b => b.SaldoTotal).HasPrecision(18, 2);
            modelBuilder.Entity<Billetera>().Property(b => b.SaldoRetenido).HasPrecision(18, 2);
            modelBuilder.Entity<Billetera>().Property(b => b.SaldoDisponible).HasPrecision(18, 2);

            modelBuilder.Entity<Subasta>().Property(s => s.PrecioBase).HasPrecision(18, 2);
            modelBuilder.Entity<Subasta>().Property(s => s.IncrementoMinimo).HasPrecision(18, 2);

            modelBuilder.Entity<Puja>().Property(p => p.Monto).HasPrecision(18, 2);
            modelBuilder.Entity<TransaccionLedger>().Property(t => t.Monto).HasPrecision(18, 2);


            // --- DATOS SEMILLA (SEED DATA) ---
            var ahora = DateTime.UtcNow;

            // 1. Usuarios obligatorios
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nombre = "Vendedor", Email = "vendedor@test.com", PasswordHash = "hash_dummy", FechaRegistro = ahora },
                new Usuario { Id = 2, Nombre = "Comprador 1", Email = "comprador1@test.com", PasswordHash = "hash_dummy", FechaRegistro = ahora },
                new Usuario { Id = 3, Nombre = "Comprador 2", Email = "comprador2@test.com", PasswordHash = "hash_dummy", FechaRegistro = ahora },
                new Usuario { Id = 4, Nombre = "Sin Fondos", Email = "sinfondos@test.com", PasswordHash = "hash_dummy", FechaRegistro = ahora }
            );

            // 2. Billeteras con los saldos exactos exigidos por el TP
            modelBuilder.Entity<Billetera>().HasData(
                new Billetera { Id = 1, UsuarioId = 1, SaldoTotal = 0, SaldoRetenido = 0, SaldoDisponible = 0, Version = 1 },
                new Billetera { Id = 2, UsuarioId = 2, SaldoTotal = 150000m, SaldoRetenido = 45000m, SaldoDisponible = 105000m, Version = 1 },
                new Billetera { Id = 3, UsuarioId = 3, SaldoTotal = 200000m, SaldoRetenido = 0, SaldoDisponible = 200000m, Version = 1 },
                new Billetera { Id = 4, UsuarioId = 4, SaldoTotal = 500m, SaldoRetenido = 0, SaldoDisponible = 500m, Version = 1 }
            );

            // 3. Categorías
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Tecnología", UrlIcono = "icon-tech" },
                new Categoria { Id = 2, Nombre = "Coleccionables", UrlIcono = "icon-col" },
                new Categoria { Id = 3, Nombre = "Indumentaria", UrlIcono = "icon-ropa" },
                new Categoria { Id = 4, Nombre = "Vehículos", UrlIcono = "icon-auto" }
            );

            // 4. Subastas con los 5 casos de prueba obligatorios
            modelBuilder.Entity<Subasta>().HasData(
                // Caso A: Activa estándar (Cierra en 30 min)
                new Subasta { Id = 1, VendedorId = 1, CategoriaId = 1, Titulo = "PlayStation 5", Descripcion = "Consola", UrlImagen = "url", PrecioBase = 10000m, IncrementoMinimo = 1000m, FechaInicio = ahora.AddHours(-1), FechaFin = ahora.AddMinutes(30), Estado = EstadoSubasta.Activa, Version = 1 },
                // Caso B: Activa crítica (Cierra en menos de 2 min)
                new Subasta { Id = 2, VendedorId = 1, CategoriaId = 1, Titulo = "iPhone 13", Descripcion = "Celular", UrlImagen = "url", PrecioBase = 5000m, IncrementoMinimo = 500m, FechaInicio = ahora.AddHours(-2), FechaFin = ahora.AddMinutes(1), Estado = EstadoSubasta.Activa, Version = 1 },
                // Caso C: Próxima (Inicio a +24 hs)
                new Subasta { Id = 3, VendedorId = 1, CategoriaId = 2, Titulo = "Figura Acción", Descripcion = "Juguete", UrlImagen = "url", PrecioBase = 20000m, IncrementoMinimo = 2000m, FechaInicio = ahora.AddHours(24), FechaFin = ahora.AddHours(48), Estado = EstadoSubasta.Programada, Version = 1 },
                // Caso D: Vencida con ganador (Fecha fin pasada para que el worker la liquide)
                new Subasta { Id = 4, VendedorId = 1, CategoriaId = 3, Titulo = "Camiseta", Descripcion = "Deportiva", UrlImagen = "url", PrecioBase = 30000m, IncrementoMinimo = 1500m, FechaInicio = ahora.AddDays(-2), FechaFin = ahora.AddMinutes(-10), Estado = EstadoSubasta.Activa, Version = 1 },
                // Caso E: Vencida desierta (Sin pujas, para que el worker la declare DESIERTA)
                new Subasta { Id = 5, VendedorId = 1, CategoriaId = 4, Titulo = "Bicicleta", Descripcion = "Rodado 29", UrlImagen = "url", PrecioBase = 50000m, IncrementoMinimo = 5000m, FechaInicio = ahora.AddDays(-2), FechaFin = ahora.AddMinutes(-30), Estado = EstadoSubasta.Activa, Version = 1 }
            );

            // 5. Pujas previas para la subasta activa y la subasta ganada
            modelBuilder.Entity<Puja>().HasData(
                // 2 pujas previas cargadas en la subasta activa estándar, siendo el líder Comprador 1 con 45.000
                new Puja { Id = 1, SubastaId = 1, CompradorId = 3, Monto = 40000m, FechaPuja = ahora.AddMinutes(-15) },
                new Puja { Id = 2, SubastaId = 1, CompradorId = 2, Monto = 45000m, FechaPuja = ahora.AddMinutes(-5) },

                // Puja ganadora de la subasta vencida (Caso D)
                new Puja { Id = 3, SubastaId = 4, CompradorId = 2, Monto = 35000m, FechaPuja = ahora.AddDays(-1) }
            );


        }
    }
}
