using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class TgMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TgMedia_UrlTgId",
                table: "Medias",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TgMedia_VistaPreviaCuadradoTgId",
                table: "Medias",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TgMedia_VistaPreviaTgId",
                table: "Medias",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TgMedia_UrlTgId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "TgMedia_VistaPreviaCuadradoTgId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "TgMedia_VistaPreviaTgId",
                table: "Medias");
        }
    }
}
