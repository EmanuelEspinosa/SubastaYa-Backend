using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SubastaYa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlIcono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Entidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    DetalleJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditoriaLogs_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Billeteras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    SaldoTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SaldoRetenido = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SaldoDisponible = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billeteras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Billeteras_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subastas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendedorId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioBase = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IncrementoMinimo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subastas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subastas_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subastas_Usuarios_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pujas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubastaId = table.Column<int>(type: "int", nullable: false),
                    CompradorId = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaPuja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pujas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pujas_Subastas_SubastaId",
                        column: x => x.SubastaId,
                        principalTable: "Subastas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pujas_Usuarios_CompradorId",
                        column: x => x.CompradorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransaccionesLedger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BilleteraId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubastaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaccionesLedger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaccionesLedger_Billeteras_BilleteraId",
                        column: x => x.BilleteraId,
                        principalTable: "Billeteras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransaccionesLedger_Subastas_SubastaId",
                        column: x => x.SubastaId,
                        principalTable: "Subastas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    { 1, "vendedor@test.com", new DateTime(2026, 9, 8, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), "Vendedor", "hash_dummy" },
                    { 2, "comprador1@test.com", new DateTime(2026, 9, 8, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), "Comprador 1", "hash_dummy" },
                    { 3, "comprador2@test.com", new DateTime(2026, 9, 8, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), "Comprador 2", "hash_dummy" },
                    { 4, "sinfondos@test.com", new DateTime(2026, 9, 8, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), "Sin Fondos", "hash_dummy" }
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
                    { 1, 1, "Consola", 2, new DateTime(2026, 9, 8, 3, 7, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 8, 1, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), 1000m, 10000m, "PlayStation 5", "url", 1, 1 },
                    { 2, 1, "Celular", 2, new DateTime(2026, 9, 8, 2, 38, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 8, 0, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), 500m, 5000m, "iPhone 13", "url", 1, 1 },
                    { 3, 2, "Juguete", 1, new DateTime(2026, 9, 10, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 9, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), 2000m, 20000m, "Figura Acción", "url", 1, 1 },
                    { 4, 3, "Deportiva", 2, new DateTime(2026, 9, 8, 2, 27, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 6, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), 1500m, 30000m, "Camiseta", "url", 1, 1 },
                    { 5, 4, "Rodado 29", 2, new DateTime(2026, 9, 8, 2, 7, 54, 296, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 9, 6, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), 5000m, 50000m, "Bicicleta", "url", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Pujas",
                columns: new[] { "Id", "CompradorId", "FechaPuja", "Monto", "SubastaId" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2026, 9, 8, 2, 22, 54, 296, DateTimeKind.Utc).AddTicks(2323), 40000m, 1 },
                    { 2, 2, new DateTime(2026, 9, 8, 2, 32, 54, 296, DateTimeKind.Utc).AddTicks(2323), 45000m, 1 },
                    { 3, 2, new DateTime(2026, 9, 7, 2, 37, 54, 296, DateTimeKind.Utc).AddTicks(2323), 35000m, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_UsuarioId",
                table: "AuditoriaLogs",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Billeteras_UsuarioId",
                table: "Billeteras",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pujas_CompradorId",
                table: "Pujas",
                column: "CompradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pujas_SubastaId",
                table: "Pujas",
                column: "SubastaId");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_CategoriaId",
                table: "Subastas",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_VendedorId",
                table: "Subastas",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesLedger_BilleteraId",
                table: "TransaccionesLedger",
                column: "BilleteraId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesLedger_SubastaId",
                table: "TransaccionesLedger",
                column: "SubastaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaLogs");

            migrationBuilder.DropTable(
                name: "Pujas");

            migrationBuilder.DropTable(
                name: "TransaccionesLedger");

            migrationBuilder.DropTable(
                name: "Billeteras");

            migrationBuilder.DropTable(
                name: "Subastas");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
