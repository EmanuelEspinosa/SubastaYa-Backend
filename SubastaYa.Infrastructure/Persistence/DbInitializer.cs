using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Enums;
using SubastaYa.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace SubastaYa.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(SubastaYaDbContext context)
        {
            // 1. Si ya existen usuarios en la base de datos, no volvemos a sembrar nada
            if (await context.Usuarios.AnyAsync()) return;

            // 1.1. Reiniciamos el contador autoincremental a 0 para que el primer registro sea ID = 1
            await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.Usuarios', RESEED, 0)");
            await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.Categorias', RESEED, 0)");
            await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.Billeteras', RESEED, 0)");
            await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.Subastas', RESEED, 0)");
            await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.Pujas', RESEED, 0)");
            await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.TransaccionesLedger', RESEED, 0)");

            var ahora = DateTime.UtcNow;
            string passwordHashDefault = "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G";

            // 2. Usuarios Semilla (sin Id)
            var usuarios = new[]
            {
                new Usuario { Nombre = "Vendedor", Email = "vendedor@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora.AddDays(-10) },
                new Usuario { Nombre = "Comprador 1", Email = "comprador1@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora.AddDays(-10) },
                new Usuario { Nombre = "Comprador 2", Email = "comprador2@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora.AddDays(-10) },
                new Usuario { Nombre = "Sin Fondos", Email = "sinfondos@test.com", PasswordHash = passwordHashDefault, FechaRegistro = ahora.AddDays(-10) }
            };
            await context.Usuarios.AddRangeAsync(usuarios);
            await context.SaveChangesAsync(); // Se asignan IDs a usuarios (1, 2, 3, 4)

            // 3. Categorías (sin Id)
            var categorias = new[]
            {
                new Categoria { Nombre = "Tecnología", UrlIcono = "icon-tech" },
                new Categoria { Nombre = "Coleccionables", UrlIcono = "icon-col" },
                new Categoria { Nombre = "Indumentaria", UrlIcono = "icon-ropa" },
                new Categoria { Nombre = "Vehículos", UrlIcono = "icon-auto" }
            };
            await context.Categorias.AddRangeAsync(categorias);
            await context.SaveChangesAsync(); // Se asignan IDs a categorías (1, 2, 3, 4)

            // 4. Billeteras Semilla (asociadas a los IDs generados de usuarios)
            var billeteras = new[]
            {
                new Billetera { UsuarioId = usuarios[0].Id, SaldoTotal = 0, SaldoRetenido = 0, SaldoDisponible = 0, Version = 1 },
                new Billetera { UsuarioId = usuarios[1].Id, SaldoTotal = 150000m, SaldoRetenido = 45000m, SaldoDisponible = 105000m, Version = 1 },
                new Billetera { UsuarioId = usuarios[2].Id, SaldoTotal = 200000m, SaldoRetenido = 0, SaldoDisponible = 200000m, Version = 1 },
                new Billetera { UsuarioId = usuarios[3].Id, SaldoTotal = 500m, SaldoRetenido = 0, SaldoDisponible = 500m, Version = 1 }
            };
            await context.Billeteras.AddRangeAsync(billeteras);
            await context.SaveChangesAsync(); // Se asignan IDs a billeteras

            // 5. Subastas con fechas calculadas en tiempo de ejecución (sin Id)
            var subastas = new[]
            {
                // Caso A: Activa estándar (cierra en 7 días)
                new Subasta { VendedorId = usuarios[0].Id, CategoriaId = categorias[0].Id, Titulo = "PlayStation 5", Descripcion = "Disfrutá de una carga instantánea gracias a su SSD de alta velocidad de 825GB. Olvidate de las esperas y sumergite directamente en la acción con gráficos impresionantes en 4K y tecnología Ray Tracing, que añade un realismo asombroso con sombras y reflejos fieles a la realidad.", UrlImagen = "https://i.ibb.co/jPwDCZ16/descarga-2026-03-31-T111030-325.png", PrecioBase = 10000m, IncrementoMinimo = 1000m, FechaInicio = ahora.AddDays(-1), FechaFin = ahora.AddDays(7), Estado = EstadoSubasta.Activa, Version = 1 },
                
                // Caso B: Activa crítica (cierra en 15 días)
                new Subasta { VendedorId = usuarios[0].Id, CategoriaId = categorias[0].Id, Titulo = "Apple iPhone 13", Descripcion = "Elevá tu experiencia tecnológica con el Apple iPhone 13 (128 GB) en un sofisticado color Azul. Este smartphone combina un rendimiento excepcional con un diseño elegante y funciones avanzadas para que disfrutes de cada momento al máximo. Chip A15 Bionic Súper Rápido. Sistema de Cámaras Doble Avanzado. Pantalla Super Retina XDR Brillante de 6.1 pulgadas.", UrlImagen = "https://i.ibb.co/NvpNhBw/2b9cec46-e3fb-4fd3-b83e-6093aa11f3ef.webp", PrecioBase = 5000m, IncrementoMinimo = 500m, FechaInicio = ahora.AddDays(-1), FechaFin = ahora.AddDays(15), Estado = EstadoSubasta.Activa, Version = 1 },
                
                // Caso C: Próxima (inicia en 2 días)
                new Subasta { VendedorId = usuarios[0].Id, CategoriaId = categorias[1].Id, Titulo = "Figura De Accion Articulada", Descripcion = "Figuras de acción articuladas con sus accesorios de lucha listos para el combate. Dimensiones: 30 cm. Personajes: HUNTSMAN, WHITE SHADE, SKULL CRACKER.", UrlImagen = "https://i.ibb.co/SXyKgwsB/200240-800-auto.webp", PrecioBase = 20000m, IncrementoMinimo = 2000m, FechaInicio = ahora.AddDays(2), FechaFin = ahora.AddDays(10), Estado = EstadoSubasta.Programada, Version = 1 },
                
                // Caso D: Vencida con ganador (venció ayer)
                new Subasta { VendedorId = usuarios[0].Id, CategoriaId = categorias[2].Id, Titulo = "Camiseta de Fútbol", Descripcion = "Confeccionada con materiales reciclados y tecnología AEROREADY para mantener la comodidad y frescura en todo momento. Ajustado. Cuello en V. 100% poliéster reciclado. AEROREADY. 8 Tiras aplicadas. Escudo de Boca Juniors tejido. Detalle “1905” aplicado en la nuca. Franja icónica dorada.", UrlImagen = "https://i.ibb.co/chydTrMW/JJ4286-01.webp", PrecioBase = 30000m, IncrementoMinimo = 1500m, FechaInicio = ahora.AddDays(-5), FechaFin = ahora.AddDays(-1), Estado = EstadoSubasta.Activa, Version = 1 },
                
                // Caso E: Vencida desierta (venció hace 2 días)
                new Subasta { VendedorId = usuarios[0].Id, CategoriaId = categorias[3].Id, Titulo = "Bicicleta Kanji R/29", Descripcion = "La Kanji ON WHEELS Rodado 29 es una bicicleta de montaña diseñada para quienes buscan rendimiento, comodidad y durabilidad en cada recorrido. Con un diseño moderno y componentes confiables, es ideal tanto para caminos urbanos exigentes como para aventuras off-road.", UrlImagen = "https://i.ibb.co/d4yNsNSs/840-kaj-289.webp", PrecioBase = 50000m, IncrementoMinimo = 5000m, FechaInicio = ahora.AddDays(-5), FechaFin = ahora.AddDays(-2), Estado = EstadoSubasta.Activa, Version = 1 }
            };
            await context.Subastas.AddRangeAsync(subastas);
            await context.SaveChangesAsync(); // Se asignan IDs a subastas

            // 6. Trazabilidad contable completa en el Ledger (sin Id)
            var transacciones = new[]
            {
                new TransaccionLedger { BilleteraId = billeteras[1].Id, Tipo = TipoTransaccionLedger.Deposito, Monto = 150000m, Fecha = ahora.AddDays(-5) },
                new TransaccionLedger { BilleteraId = billeteras[1].Id, Tipo = TipoTransaccionLedger.Retencion, Monto = 45000m, Fecha = ahora.AddHours(-1), SubastaId = subastas[0].Id },

                // Historial conciliado de Comprador 2
                new TransaccionLedger { BilleteraId = billeteras[2].Id, Tipo = TipoTransaccionLedger.Deposito, Monto = 200000m, Fecha = ahora.AddDays(-5) },
                new TransaccionLedger { BilleteraId = billeteras[2].Id, Tipo = TipoTransaccionLedger.Retencion, Monto = 40000m, Fecha = ahora.AddHours(-3), SubastaId = subastas[0].Id },
                new TransaccionLedger { BilleteraId = billeteras[2].Id, Tipo = TipoTransaccionLedger.Liberacion, Monto = 40000m, Fecha = ahora.AddHours(-1), SubastaId = subastas[0].Id },

                new TransaccionLedger { BilleteraId = billeteras[3].Id, Tipo = TipoTransaccionLedger.Deposito, Monto = 500m, Fecha = ahora.AddDays(-5) }
            };
            await context.TransaccionesLedger.AddRangeAsync(transacciones);

            // 7. Pujas (sin Id)
            var pujas = new[]
            {
                new Puja { SubastaId = subastas[0].Id, CompradorId = usuarios[2].Id, Monto = 40000m, FechaPuja = ahora.AddHours(-3) },
                new Puja { SubastaId = subastas[0].Id, CompradorId = usuarios[1].Id, Monto = 45000m, FechaPuja = ahora.AddHours(-1) },
                new Puja { SubastaId = subastas[3].Id, CompradorId = usuarios[1].Id, Monto = 35000m, FechaPuja = ahora.AddDays(-2) }
            };
            await context.Pujas.AddRangeAsync(pujas);

            // Guardar cambios finales
            await context.SaveChangesAsync();
        }
    }
}
