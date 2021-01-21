using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Encuestas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Encuesta",
                table: "Hilos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Encuesta",
                table: "Hilos");
        }
    }
}
