using Microsoft.EntityFrameworkCore;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using System;

namespace SubastaYa.Infrastructure.Context
{
    public class SubastaYaDbContext : DbContext
    {
        public SubastaYaDbContext(DbContextOptions<SubastaYaDbContext> options)
            : base(options)
        {
        }

        // DbSets para las tablas de la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Billetera> Billeteras { get; set; }
        public DbSet<Subasta> Subastas { get; set; }
        public DbSet<Puja> Pujas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TransaccionLedger> TransaccionesLedger { get; set; }
        public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================================================================
            // 1. RELACIONES Y SOLUCIÓN ERROR 1785 (Evitar ciclos de cascada en SQL Server)
            // =========================================================================

            // Relación 1 a 1: Usuario <-> Billetera
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Billetera)
                .WithOne(b => b.Usuario)
                .HasForeignKey<Billetera>(b => b.UsuarioId);

            // Relaciones de Puja (CLAVE PARA EL ERROR 1785):
            modelBuilder.Entity<Puja>(entity =>
            {
                // Con Comprador: Restrict para que no choque con el camino de Subasta/Vendedor
                entity.HasOne(p => p.Comprador)
                      .WithMany(u => u.PujasRealizadas)
                      .HasForeignKey(p => p.CompradorId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Con Subasta: Cascade para que si se borra una subasta se borren sus pujas
                entity.HasOne(p => p.Subasta)
                      .WithMany(s => s.Pujas)
                      .HasForeignKey(p => p.SubastaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Relación de Subasta con Vendedor (Restrict para evitar borrado accidental)
            modelBuilder.Entity<Subasta>(entity =>
            {
                entity.HasOne(s => s.Vendedor)
                      .WithMany(u => u.SubastasPublicadas)
                      .HasForeignKey(s => s.VendedorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Relación de TransaccionLedger con Subasta (Nullable / Restrict)
            modelBuilder.Entity<TransaccionLedger>(entity =>
            {
                entity.HasOne(t => t.Subasta)
                      .WithMany(s => s.TransaccionesLedger)
                      .HasForeignKey(t => t.SubastaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Email de Usuario debe ser único en la base de datos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // =========================================================================
            // 2. CONCURRENCIA OPTIMISTA (Optimistic Locking - Requisito estricto del TP)
            // =========================================================================
            modelBuilder.Entity<Subasta>()
                .Property(s => s.Version)
                .IsConcurrencyToken();

            modelBuilder.Entity<Billetera>()
                .Property(b => b.Version)
                .IsConcurrencyToken();

            // =========================================================================
            // 3. PRECISIÓN DECIMAL (Moneda estándar decimal(18,2))
            // =========================================================================
            modelBuilder.Entity<Billetera>().Property(b => b.SaldoTotal).HasPrecision(18, 2);
            modelBuilder.Entity<Billetera>().Property(b => b.SaldoRetenido).HasPrecision(18, 2);
            modelBuilder.Entity<Billetera>().Property(b => b.SaldoDisponible).HasPrecision(18, 2);

            modelBuilder.Entity<Subasta>().Property(s => s.PrecioBase).HasPrecision(18, 2);
            modelBuilder.Entity<Subasta>().Property(s => s.IncrementoMinimo).HasPrecision(18, 2);

            modelBuilder.Entity<Puja>().Property(p => p.Monto).HasPrecision(18, 2);
            modelBuilder.Entity<TransaccionLedger>().Property(t => t.Monto).HasPrecision(18, 2);

            // =========================================================================
            // 4. DATOS SEMILLA (SEED DATA OBLIGATORIO)
            // =========================================================================
            var ahora = DateTime.UtcNow;

            // Hash oficial de BCrypt para la contraseña "123456"
            string passwordHashDefault = "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G";

            // Usuarios obligatorios con contraseña "123456"
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nombre = "Vendedor", Email = "vendedor@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora },
                new Usuario { Id = 2, Nombre = "Comprador 1", Email = "comprador1@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora },
                new Usuario { Id = 3, Nombre = "Comprador 2", Email = "comprador2@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora },
                new Usuario { Id = 4, Nombre = "Sin Fondos", Email = "sinfondos@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora }
            );

            // Billeteras con los saldos exactos exigidos por el TP
            modelBuilder.Entity<Billetera>().HasData(
                new Billetera { Id = 1, UsuarioId = 1, SaldoTotal = 0, SaldoRetenido = 0, SaldoDisponible = 0, Version = 1 },
                new Billetera { Id = 2, UsuarioId = 2, SaldoTotal = 150000m, SaldoRetenido = 45000m, SaldoDisponible = 105000m, Version = 1 },
                new Billetera { Id = 3, UsuarioId = 3, SaldoTotal = 200000m, SaldoRetenido = 0, SaldoDisponible = 200000m, Version = 1 },
                new Billetera { Id = 4, UsuarioId = 4, SaldoTotal = 500m, SaldoRetenido = 0, SaldoDisponible = 500m, Version = 1 }
            );

            // Categorías
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Tecnología", UrlIcono = "icon-tech" },
                new Categoria { Id = 2, Nombre = "Coleccionables", UrlIcono = "icon-col" },
                new Categoria { Id = 3, Nombre = "Indumentaria", UrlIcono = "icon-ropa" },
                new Categoria { Id = 4, Nombre = "Vehículos", UrlIcono = "icon-auto" }
            );

            // Subastas con los 5 casos de prueba obligatorios
            modelBuilder.Entity<Subasta>().HasData(
                // Caso A: Activa estándar (Cierra en 30 min)
                new Subasta { Id = 1, VendedorId = 1, CategoriaId = 1, Titulo = "PlayStation 5", Descripcion = "Disfrutá de una carga instantánea gracias a su SSD de alta velocidad de 825GB. Olvidate de las esperas y sumergite directamente en la acción con gráficos impresionantes en 4K y tecnología Ray Tracing, que añade un realismo asombroso con sombras y reflejos fieles a la realidad.", UrlImagen = "https://i.ibb.co/jPwDCZ16/descarga-2026-03-31-T111030-325.png", PrecioBase = 10000m, IncrementoMinimo = 1000m, FechaInicio = ahora.AddHours(-1), FechaFin = ahora.AddMinutes(30), Estado = EstadoSubasta.Activa, Version = 1 },
                // Caso B: Activa crítica (Cierra en menos de 2 min)
                new Subasta { Id = 2, VendedorId = 1, CategoriaId = 1, Titulo = "Apple iPhone 13", Descripcion = "Elevá tu experiencia tecnológica con el Apple iPhone 13 (128 GB) en un sofisticado color Azul. Este smartphone combina un rendimiento excepcional con un diseño elegante y funciones avanzadas para que disfrutes de cada momento al máximo. Chip A15 Bionic Súper Rápido. Sistema de Cámaras Doble Avanzado. Pantalla Super Retina XDR Brillante de 6.1 pulgadas.", UrlImagen = "https://i.ibb.co/NvpNhBw/2b9cec46-e3fb-4fd3-b83e-6093aa11f3ef.webp", PrecioBase = 5000m, IncrementoMinimo = 500m, FechaInicio = ahora.AddHours(-2), FechaFin = ahora.AddMinutes(1), Estado = EstadoSubasta.Activa, Version = 1 },
                // Caso C: Próxima (Inicio a +24 hs)
                new Subasta { Id = 3, VendedorId = 1, CategoriaId = 2, Titulo = "Figura De Accion Articulada", Descripcion = "Figuras de acción articuladas con sus accesorios de lucha listos para el combate. Dimensiones: 30 cm. Personajes: HUNTSMAN, WHITE SHADE, SKULL CRACKER.", UrlImagen = "https://i.ibb.co/SXyKgwsB/200240-800-auto.webp", PrecioBase = 20000m, IncrementoMinimo = 2000m, FechaInicio = ahora.AddHours(24), FechaFin = ahora.AddHours(48), Estado = EstadoSubasta.Programada, Version = 1 },
                // Caso D: Vencida con ganador (Fecha fin pasada para que el worker la liquide)
                new Subasta { Id = 4, VendedorId = 1, CategoriaId = 3, Titulo = "Camiseta de Fútbol", Descripcion = "Confeccionada con materiales reciclados y tecnología AEROREADY para mantener la comodidad y frescura en todo momento. Ajustado. Cuello en V. 100% poliéster reciclado. AEROREADY. 8 Tiras aplicadas. Escudo de Boca Juniors tejido. Detalle “1905” aplicado en la nuca. Franja icónica dorada.", UrlImagen = "https://i.ibb.co/chydTrMW/JJ4286-01.webp", PrecioBase = 30000m, IncrementoMinimo = 1500m, FechaInicio = ahora.AddDays(-2), FechaFin = ahora.AddMinutes(-10), Estado = EstadoSubasta.Activa, Version = 1 },
                // Caso E: Vencida desierta (Sin pujas, para que el worker la declare DESIERTA)
                new Subasta { Id = 5, VendedorId = 1, CategoriaId = 4, Titulo = "Bicicleta Kanji R/29", Descripcion = "La Kanji ON WHEELS Rodado 29 es una bicicleta de montaña diseñada para quienes buscan rendimiento, comodidad y durabilidad en cada recorrido. Con un diseño moderno y componentes confiables, es ideal tanto para caminos urbanos exigentes como para aventuras off-road.", UrlImagen = "https://i.ibb.co/d4yNsNSs/840-kaj-289.webp", PrecioBase = 50000m, IncrementoMinimo = 5000m, FechaInicio = ahora.AddDays(-2), FechaFin = ahora.AddMinutes(-30), Estado = EstadoSubasta.Activa, Version = 1 }
            );

            // 6. Transacciones iniciales del Libro Mayor (Ledger) para justificar los saldos y retenciones
            modelBuilder.Entity<TransaccionLedger>().HasData(
                // Depósito inicial de Comprador 1 ($150.000)
                new TransaccionLedger { Id = 1, BilleteraId = 2, Tipo = TipoTransaccionLedger.Deposito, Monto = 150000m, Fecha = ahora.AddDays(-1) },
                // Retención inicial por su puja líder en la Subasta 1 ($45.000)
                new TransaccionLedger { Id = 2, BilleteraId = 2, Tipo = TipoTransaccionLedger.Retencion, Monto = 45000m, Fecha = ahora.AddMinutes(-5), SubastaId = 1 },

                // Depósito inicial de Comprador 2 ($200.000)
                new TransaccionLedger { Id = 3, BilleteraId = 3, Tipo = TipoTransaccionLedger.Deposito, Monto = 200000m, Fecha = ahora.AddDays(-1) },

                // Depósito inicial de Sin Fondos ($500)
                new TransaccionLedger { Id = 4, BilleteraId = 4, Tipo = TipoTransaccionLedger.Deposito, Monto = 500m, Fecha = ahora.AddDays(-1) }
            );


            // Pujas previas para la subasta activa y la subasta ganada
            modelBuilder.Entity<Puja>().HasData(
                new Puja { Id = 1, SubastaId = 1, CompradorId = 3, Monto = 40000m, FechaPuja = ahora.AddMinutes(-15) },
                new Puja { Id = 2, SubastaId = 1, CompradorId = 2, Monto = 45000m, FechaPuja = ahora.AddMinutes(-5) },
                new Puja { Id = 3, SubastaId = 4, CompradorId = 2, Monto = 35000m, FechaPuja = ahora.AddDays(-1) }
            );
        }
    }
}