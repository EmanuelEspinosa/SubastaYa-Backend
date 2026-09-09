using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SubastaYa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedLedgerInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 9, 18, 53, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 9, 17, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 9, 18, 24, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 9, 16, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 11, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 10, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 9, 18, 13, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 7, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 9, 17, 53, 37, 150, DateTimeKind.Utc).AddTicks(2647), new DateTime(2026, 9, 7, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647) });

            migrationBuilder.InsertData(
                table: "TransaccionesLedger",
                columns: new[] { "Id", "BilleteraId", "Fecha", "Monto", "SubastaId", "Tipo" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2026, 9, 8, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), 150000m, null, 1 },
                    { 2, 2, new DateTime(2026, 9, 9, 18, 18, 37, 150, DateTimeKind.Utc).AddTicks(2647), 45000m, 1, 2 },
                    { 3, 3, new DateTime(2026, 9, 8, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), 200000m, null, 1 },
                    { 4, 4, new DateTime(2026, 9, 8, 18, 23, 37, 150, DateTimeKind.Utc).AddTicks(2647), 500m, null, 1 }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 8, 2, 22, 54, 296, DateTimeKind.Utc).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 8, 2, 32, 54, 296, DateTimeKind.Utc).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 7, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 8, 3, 7, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 8, 1, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 8, 2, 38, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 8, 0, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 10, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 9, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 8, 2, 27, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 6, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 8, 2, 7, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 6, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323) });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 8, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 8, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 8, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaRegistro",
                value: new DateTime(2026, 9, 8, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323));
        }
    }
}
