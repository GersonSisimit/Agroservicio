using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agroservicio.Migrations
{
    /// <inheritdoc />
    public partial class TablesForClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DireccionCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DireccionCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DireccionCliente_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    IdDireccionCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factura_DireccionCliente_IdDireccionCliente",
                        column: x => x.IdDireccionCliente,
                        principalTable: "DireccionCliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineaFactura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFactura = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<double>(type: "float", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    SubTotal = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineaFactura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineaFactura_Factura_IdFactura",
                        column: x => x.IdFactura,
                        principalTable: "Factura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineaFactura_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DireccionCliente_IdCliente",
                table: "DireccionCliente",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_IdDireccionCliente",
                table: "Factura",
                column: "IdDireccionCliente");

            migrationBuilder.CreateIndex(
                name: "IX_LineaFactura_IdFactura",
                table: "LineaFactura",
                column: "IdFactura");

            migrationBuilder.CreateIndex(
                name: "IX_LineaFactura_IdProducto",
                table: "LineaFactura",
                column: "IdProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineaFactura");

            migrationBuilder.DropTable(
                name: "Factura");

            migrationBuilder.DropTable(
                name: "DireccionCliente");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
