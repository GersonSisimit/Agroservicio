using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agroservicio.Migrations
{
    /// <inheritdoc />
    public partial class EliminarColumna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "I",
                table: "TipoProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "I",
                table: "TipoProducto",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
