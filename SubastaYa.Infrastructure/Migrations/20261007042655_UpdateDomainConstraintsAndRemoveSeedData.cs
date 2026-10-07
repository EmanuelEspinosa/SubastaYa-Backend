using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SubastaYa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDomainConstraintsAndRemoveSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Billeteras",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Billeteras",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Billeteras",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Billeteras",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AddCheckConstraint(
                name: "CK_TransaccionLedger_Monto_MayorACero",
                table: "TransaccionesLedger",
                sql: "[Monto] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Subasta_IncrementoMinimo_MayorACero",
                table: "Subastas",
                sql: "[IncrementoMinimo] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Subasta_PrecioBase_Positivo",
                table: "Subastas",
                sql: "[PrecioBase] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Puja_Monto_MayorACero",
                table: "Pujas",
                sql: "[Monto] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Billetera_SaldoDisponible_Positivo",
                table: "Billeteras",
                sql: "[SaldoDisponible] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Billetera_SaldoRetenido_Positivo",
                table: "Billeteras",
                sql: "[SaldoRetenido] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Billetera_SaldoTotal_Positivo",
                table: "Billeteras",
                sql: "[SaldoTotal] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_TransaccionLedger_Monto_MayorACero",
                table: "TransaccionesLedger");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Subasta_IncrementoMinimo_MayorACero",
                table: "Subastas");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Subasta_PrecioBase_Positivo",
                table: "Subastas");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Puja_Monto_MayorACero",
                table: "Pujas");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Billetera_SaldoDisponible_Positivo",
                table: "Billeteras");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Billetera_SaldoRetenido_Positivo",
                table: "Billeteras");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Billetera_SaldoTotal_Positivo",
                table: "Billeteras");

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Nombre", "UrlIcono" },
                values: new object[,]
                {
                    { 1, "Tecnología", "icon-tech" },
                    { 2, "Coleccionables", "icon-col" },
                    { 3, "Indumentaria", "icon-ropa" },
                    { 4, "Vehículos", "icon-auto" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "FechaRegistro", "Nombre", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "vendedor@test.com", new DateTime(2026, 9, 15, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), "Vendedor", "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G" },
                    { 2, "comprador1@test.com", new DateTime(2026, 9, 15, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), "Comprador 1", "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G" },
                    { 3, "comprador2@test.com", new DateTime(2026, 9, 15, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), "Comprador 2", "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G" },
                    { 4, "sinfondos@test.com", new DateTime(2026, 9, 15, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), "Sin Fondos", "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G" }
                });

            migrationBuilder.InsertData(
                table: "Billeteras",
                columns: new[] { "Id", "SaldoDisponible", "SaldoRetenido", "SaldoTotal", "UsuarioId", "Version" },
                values: new object[,]
                {
                    { 1, 0m, 0m, 0m, 1, 1 },
                    { 2, 105000m, 45000m, 150000m, 2, 1 },
                    { 3, 200000m, 0m, 200000m, 3, 1 },
                    { 4, 500m, 0m, 500m, 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "Subastas",
                columns: new[] { "Id", "CategoriaId", "Descripcion", "Estado", "FechaFin", "FechaInicio", "IncrementoMinimo", "PrecioBase", "Titulo", "UrlImagen", "VendedorId", "Version" },
                values: new object[,]
                {
                    { 1, 1, "Disfrutá de una carga instantánea gracias a su SSD de alta velocidad de 825GB. Olvidate de las esperas y sumergite directamente en la acción con gráficos impresionantes en 4K y tecnología Ray Tracing, que añade un realismo asombroso con sombras y reflejos fieles a la realidad.", 2, new DateTime(2026, 9, 15, 6, 16, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 15, 4, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), 1000m, 10000m, "PlayStation 5", "https://i.ibb.co/jPwDCZ16/descarga-2026-03-31-T111030-325.png", 1, 1 },
                    { 2, 1, "Elevá tu experiencia tecnológica con el Apple iPhone 13 (128 GB) en un sofisticado color Azul. Este smartphone combina un rendimiento excepcional con un diseño elegante y funciones avanzadas para que disfrutes de cada momento al máximo. Chip A15 Bionic Súper Rápido. Sistema de Cámaras Doble Avanzado. Pantalla Super Retina XDR Brillante de 6.1 pulgadas.", 2, new DateTime(2026, 9, 15, 5, 47, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 15, 3, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), 500m, 5000m, "Apple iPhone 13", "https://i.ibb.co/NvpNhBw/2b9cec46-e3fb-4fd3-b83e-6093aa11f3ef.webp", 1, 1 },
                    { 3, 2, "Figuras de acción articuladas con sus accesorios de lucha listos para el combate. Dimensiones: 30 cm. Personajes: HUNTSMAN, WHITE SHADE, SKULL CRACKER.", 1, new DateTime(2026, 9, 17, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 16, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), 2000m, 20000m, "Figura De Accion Articulada", "https://i.ibb.co/SXyKgwsB/200240-800-auto.webp", 1, 1 },
                    { 4, 3, "Confeccionada con materiales reciclados y tecnología AEROREADY para mantener la comodidad y frescura en todo momento. Ajustado. Cuello en V. 100% poliéster reciclado. AEROREADY. 8 Tiras aplicadas. Escudo de Boca Juniors tejido. Detalle “1905” aplicado en la nuca. Franja icónica dorada.", 2, new DateTime(2026, 9, 15, 5, 36, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 13, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), 1500m, 30000m, "Camiseta de Fútbol", "https://i.ibb.co/chydTrMW/JJ4286-01.webp", 1, 1 },
                    { 5, 4, "La Kanji ON WHEELS Rodado 29 es una bicicleta de montaña diseñada para quienes buscan rendimiento, comodidad y durabilidad en cada recorrido. Con un diseño moderno y componentes confiables, es ideal tanto para caminos urbanos exigentes como para aventuras off-road.", 2, new DateTime(2026, 9, 15, 5, 16, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 13, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), 5000m, 50000m, "Bicicleta Kanji R/29", "https://i.ibb.co/d4yNsNSs/840-kaj-289.webp", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Pujas",
                columns: new[] { "Id", "CompradorId", "FechaPuja", "Monto", "SubastaId" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2026, 9, 15, 5, 31, 3, 130, DateTimeKind.Utc).AddTicks(611), 40000m, 1 },
                    { 2, 2, new DateTime(2026, 9, 15, 5, 41, 3, 130, DateTimeKind.Utc).AddTicks(611), 45000m, 1 },
                    { 3, 2, new DateTime(2026, 9, 14, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), 35000m, 4 }
                });

            migrationBuilder.InsertData(
                table: "TransaccionesLedger",
                columns: new[] { "Id", "BilleteraId", "Fecha", "Monto", "SubastaId", "Tipo" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2026, 9, 14, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), 150000m, null, 1 },
                    { 2, 2, new DateTime(2026, 9, 15, 5, 41, 3, 130, DateTimeKind.Utc).AddTicks(611), 45000m, 1, 2 },
                    { 3, 3, new DateTime(2026, 9, 14, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), 200000m, null, 1 },
                    { 4, 4, new DateTime(2026, 9, 14, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), 500m, null, 1 }
                });
        }
    }
}
