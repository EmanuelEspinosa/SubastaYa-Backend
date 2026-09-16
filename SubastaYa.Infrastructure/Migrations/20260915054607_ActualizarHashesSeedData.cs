using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubastaYa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarHashesSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 15, 5, 31, 3, 130, DateTimeKind.Utc).AddTicks(611));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 15, 5, 41, 3, 130, DateTimeKind.Utc).AddTicks(611));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 14, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611));

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 6, 16, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 15, 4, 46, 3, 130, DateTimeKind.Utc).AddTicks(611) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 47, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 15, 3, 46, 3, 130, DateTimeKind.Utc).AddTicks(611) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 17, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 16, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 36, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 13, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 16, 3, 130, DateTimeKind.Utc).AddTicks(611), new DateTime(2026, 9, 13, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611) });

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 9, 15, 5, 41, 3, 130, DateTimeKind.Utc).AddTicks(611));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 46, 3, 130, DateTimeKind.Utc).AddTicks(611), "$2a$10$21Oux/EE3yLmazVw64BjbeD6.7KKALDrKfOiWVLqBsrSlsEM7Nc9G" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 15, 5, 5, 1, 639, DateTimeKind.Utc).AddTicks(8476));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 15, 5, 15, 1, 639, DateTimeKind.Utc).AddTicks(8476));

            migrationBuilder.UpdateData(
                table: "Pujas",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaPuja",
                value: new DateTime(2026, 9, 14, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476));

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 50, 1, 639, DateTimeKind.Utc).AddTicks(8476), new DateTime(2026, 9, 15, 4, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 21, 1, 639, DateTimeKind.Utc).AddTicks(8476), new DateTime(2026, 9, 15, 3, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 17, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476), new DateTime(2026, 9, 16, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 10, 1, 639, DateTimeKind.Utc).AddTicks(8476), new DateTime(2026, 9, 13, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476) });

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2026, 9, 15, 4, 50, 1, 639, DateTimeKind.Utc).AddTicks(8476), new DateTime(2026, 9, 13, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476) });

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 9, 15, 5, 15, 1, 639, DateTimeKind.Utc).AddTicks(8476));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476));

            migrationBuilder.UpdateData(
                table: "TransaccionesLedger",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 9, 14, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476), "$2a$10$rqHJJTHsxMbtX/5ZjG1mFuWyYbUDW1PLbfwQRN0uChwes38c/0m3e" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476), "$2a$10$rqHJJTHsxMbtX/5ZjG1mFuWyYbUDW1PLbfwQRN0uChwes38c/0m3e" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476), "$2a$10$rqHJJTHsxMbtX/5ZjG1mFuWyYbUDW1PLbfwQRN0uChwes38c/0m3e" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaRegistro", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 15, 5, 20, 1, 639, DateTimeKind.Utc).AddTicks(8476), "$2a$10$rqHJJTHsxMbtX/5ZjG1mFuWyYbUDW1PLbfwQRN0uChwes38c/0m3e" });
        }
    }
}
