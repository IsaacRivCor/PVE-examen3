using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen3.Data.Migrations
{
    public partial class agregacionImagenes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "urlImagen",
                table: "Entradas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "urlImagen",
                table: "Comidas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "urlImagen",
                table: "Bebidas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "urlImagen",
                table: "Entradas");

            migrationBuilder.DropColumn(
                name: "urlImagen",
                table: "Comidas");

            migrationBuilder.DropColumn(
                name: "urlImagen",
                table: "Bebidas");
        }
    }
}
