using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agroservicio.Migrations
{
    /// <inheritdoc />
    public partial class MapeDetalleProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdTipoProducto = table.Column<int>(type: "int", nullable: false),
                    IdMarca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseProducto_Marca_IdMarca",
                        column: x => x.IdMarca,
                        principalTable: "Marca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseProducto_TipoProducto_IdTipoProducto",
                        column: x => x.IdTipoProducto,
                        principalTable: "TipoProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpaqueProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpaqueProducto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBaseProducto = table.Column<int>(type: "int", nullable: false),
                    IdEmpaqueProducto = table.Column<int>(type: "int", nullable: false),
                    RutaImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    Existencia = table.Column<double>(type: "float", nullable: false),
                    EstadoProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producto_BaseProducto_IdBaseProducto",
                        column: x => x.IdBaseProducto,
                        principalTable: "BaseProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_EmpaqueProducto_IdEmpaqueProducto",
                        column: x => x.IdEmpaqueProducto,
                        principalTable: "EmpaqueProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseProducto_IdMarca",
                table: "BaseProducto",
                column: "IdMarca");

            migrationBuilder.CreateIndex(
                name: "IX_BaseProducto_IdTipoProducto",
                table: "BaseProducto",
                column: "IdTipoProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdBaseProducto",
                table: "Producto",
                column: "IdBaseProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdEmpaqueProducto",
                table: "Producto",
                column: "IdEmpaqueProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "BaseProducto");

            migrationBuilder.DropTable(
                name: "EmpaqueProducto");
        }
    }
}
