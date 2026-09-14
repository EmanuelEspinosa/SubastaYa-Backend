using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubastaYa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarSeedSubastas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 14, 5, 48, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 14, 5, 58, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 13, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "UrlImagen" },
                values: new object[] { "Disfrutá de una carga instantánea gracias a su SSD de alta velocidad de 825GB. Olvidate de las esperas y sumergite directamente en la acción con gráficos impresionantes en 4K y tecnología Ray Tracing, que añade un realismo asombroso con sombras y reflejos fieles a la realidad.", new DateTime(2026, 9, 14, 6, 33, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 14, 5, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), "https://i.ibb.co/jPwDCZ16/descarga-2026-03-31-T111030-325.png" });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "Titulo", "UrlImagen" },
                values: new object[] { "Elevá tu experiencia tecnológica con el Apple iPhone 13 (128 GB) en un sofisticado color Azul. Este smartphone combina un rendimiento excepcional con un diseño elegante y funciones avanzadas para que disfrutes de cada momento al máximo. Chip A15 Bionic Súper Rápido. Sistema de Cámaras Doble Avanzado. Pantalla Super Retina XDR Brillante de 6.1 pulgadas.", new DateTime(2026, 9, 14, 6, 4, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 14, 4, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), "Apple iPhone 13", "https://i.ibb.co/NvpNhBw/2b9cec46-e3fb-4fd3-b83e-6093aa11f3ef.webp" });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "Titulo", "UrlImagen" },
                values: new object[] { "Figuras de acción articuladas con sus accesorios de lucha listos para el combate. Dimensiones: 30 cm. Personajes: HUNTSMAN, WHITE SHADE, SKULL CRACKER.", new DateTime(2026, 9, 16, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 15, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), "Figura De Accion Articulada", "https://i.ibb.co/SXyKgwsB/200240-800-auto.webp" });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "Titulo", "UrlImagen" },
                values: new object[] { "Confeccionada con materiales reciclados y tecnología AEROREADY para mantener la comodidad y frescura en todo momento. Ajustado. Cuello en V. 100% poliéster reciclado. AEROREADY. 8 Tiras aplicadas. Escudo de Boca Juniors tejido. Detalle “1905” aplicado en la nuca. Franja icónica dorada.", new DateTime(2026, 9, 14, 5, 53, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 12, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), "Camiseta de Fútbol", "https://i.ibb.co/chydTrMW/JJ4286-01.webp" });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "Titulo", "UrlImagen" },
                values: new object[] { "La Kanji ON WHEELS Rodado 29 es una bicicleta de montaña diseñada para quienes buscan rendimiento, comodidad y durabilidad en cada recorrido. Con un diseño moderno y componentes confiables, es ideal tanto para caminos urbanos exigentes como para aventuras off-road.", new DateTime(2026, 9, 14, 5, 33, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 12, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), "Bicicleta Kanji R/29", "https://i.ibb.co/d4yNsNSs/840-kaj-289.webp" });

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 9, 13, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 5, 58, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 9, 13, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 9, 13, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 14, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 14, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 14, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 14, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 9, 18, 8, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 9, 18, 18, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 8, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "UrlImagen" },
                values: new object[] { "Consola", new DateTime(2026, 9, 9, 18, 53, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 9, 17, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), "url" });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "Titulo", "UrlImagen" },
                values: new object[] { "Celular", new DateTime(2026, 9, 9, 18, 24, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 9, 16, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), "iPhone 13", "url" });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "Titulo", "UrlImagen" },
                values: new object[] { "Juguete", new DateTime(2026, 9, 11, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 10, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), "Figura Acción", "url" });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "Titulo", "UrlImagen" },
                values: new object[] { "Deportiva", new DateTime(2026, 9, 9, 18, 13, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 7, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), "Camiseta", "url" });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Descripcion", "FechaFin", "FechaInicio", "Titulo", "UrlImagen" },
                values: new object[] { "Rodado 29", new DateTime(2026, 9, 9, 17, 53, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 7, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), "Bicicleta", "url" });

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 9, 8, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 9, 9, 18, 18, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 9, 8, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 9, 8, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 9, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 9, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 9, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 9, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647));
        }
    }
}
