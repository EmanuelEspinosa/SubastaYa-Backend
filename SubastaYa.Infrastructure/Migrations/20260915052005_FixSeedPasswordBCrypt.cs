using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubastaYa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedPasswordBCrypt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
