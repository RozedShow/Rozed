using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class HiloFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Flags",
                table: "Hilos",
                defaultValue: "",
                
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flags",
                table: "Hilos");
        }
    }
}
