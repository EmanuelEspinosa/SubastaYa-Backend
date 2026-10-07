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
            // 3. PRECISIÓN DECIMAL Y RESTRICCIONES DE DOMINIO EN BD (CHECK CONSTRAINTS)
            // =========================================================================

            // Billetera: Saldos mayores o iguales a cero
            modelBuilder.Entity<Billetera>(entity =>
            {
                entity.Property(b => b.SaldoTotal).HasPrecision(18, 2);
                entity.Property(b => b.SaldoRetenido).HasPrecision(18, 2);
                entity.Property(b => b.SaldoDisponible).HasPrecision(18, 2);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Billetera_SaldoTotal_Positivo", "[SaldoTotal] >= 0");
                    t.HasCheckConstraint("CK_Billetera_SaldoRetenido_Positivo", "[SaldoRetenido] >= 0");
                    t.HasCheckConstraint("CK_Billetera_SaldoDisponible_Positivo", "[SaldoDisponible] >= 0");
                });
            });

            // Subasta: Precios e incrementos válidos
            modelBuilder.Entity<Subasta>(entity =>
            {
                entity.Property(s => s.PrecioBase).HasPrecision(18, 2);
                entity.Property(s => s.IncrementoMinimo).HasPrecision(18, 2);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Subasta_PrecioBase_Positivo", "[PrecioBase] >= 0");
                    t.HasCheckConstraint("CK_Subasta_IncrementoMinimo_MayorACero", "[IncrementoMinimo] > 0");
                });
            });

            // Puja: Monto de la oferta estrictamente mayor a cero
            modelBuilder.Entity<Puja>(entity =>
            {
                entity.Property(p => p.Monto).HasPrecision(18, 2);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Puja_Monto_MayorACero", "[Monto] > 0");
                });
            });

            // TransaccionLedger: Montos de depósitos / movimientos mayores a cero
            modelBuilder.Entity<TransaccionLedger>(entity =>
            {
                entity.Property(t => t.Monto).HasPrecision(18, 2);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_TransaccionLedger_Monto_MayorACero", "[Monto] > 0");
                });
            });

       
        }
    }
}