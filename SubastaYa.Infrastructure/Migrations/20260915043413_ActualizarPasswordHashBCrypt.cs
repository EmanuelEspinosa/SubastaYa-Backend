using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubastaYa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarPasswordHashBCrypt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 15, 4, 19, 10, 219, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 15, 4, 29, 10, 219, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 14, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 4, 10, 219, DateTimeKind.Utc).AddTicks(6208), new DateTime(2026, 9, 15, 3, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 4, 35, 10, 219, DateTimeKind.Utc).AddTicks(6208), new DateTime(2026, 9, 15, 2, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 17, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208), new DateTime(2026, 9, 16, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 4, 24, 10, 219, DateTimeKind.Utc).AddTicks(6208), new DateTime(2026, 9, 13, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 4, 4, 10, 219, DateTimeKind.Utc).AddTicks(6208), new DateTime(2026, 9, 13, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208) });

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 9, 15, 4, 29, 10, 219, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208), "$2a$11$e/y8Yw0P5g9A1K4v1f8Sye1O6u6Iq3q5/6W9U0.4A8S.3A2A8S.2S" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208), "$2a$11$e/y8Yw0P5g9A1K4v1f8Sye1O6u6Iq3q5/6W9U0.4A8S.3A2A8S.2S" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208), "$2a$11$e/y8Yw0P5g9A1K4v1f8Sye1O6u6Iq3q5/6W9U0.4A8S.3A2A8S.2S" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 4, 34, 10, 219, DateTimeKind.Utc).AddTicks(6208), "$2a$11$e/y8Yw0P5g9A1K4v1f8Sye1O6u6Iq3q5/6W9U0.4A8S.3A2A8S.2S" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 14, 6, 33, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 14, 5, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 14, 6, 4, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 14, 4, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 16, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 15, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 14, 5, 53, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 12, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 14, 5, 33, 28, 520, DateTimeKind.Utc).AddTicks(7152), new DateTime(2026, 9, 12, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152) });

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
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 14, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), "hash_dummy" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 14, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), "hash_dummy" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 14, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), "hash_dummy" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 14, 6, 3, 28, 520, DateTimeKind.Utc).AddTicks(7152), "hash_dummy" });
        }
    }
}
