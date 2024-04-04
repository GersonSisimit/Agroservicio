using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agroservicio.Migrations
{
    /// <inheritdoc />
    public partial class TipoProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrupoTipoProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoTipoProducto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    I = table.Column<int>(type: "int", nullable: false),
                    IdGrupoTipoProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoProducto_GrupoTipoProducto_IdGrupoTipoProducto",
                        column: x => x.IdGrupoTipoProducto,
                        principalTable: "GrupoTipoProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoProducto_IdGrupoTipoProducto",
                table: "TipoProducto",
                column: "IdGrupoTipoProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoProducto");

            migrationBuilder.DropTable(
                name: "GrupoTipoProducto");
        }
    }
}
