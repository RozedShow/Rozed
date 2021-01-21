using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class DenunciaNoNecesaria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccionesDeModeracion_Denuncias_DenunciaId",
                table: "AccionesDeModeracion");

            migrationBuilder.AddForeignKey(
                name: "FK_AccionesDeModeracion_Denuncias_DenunciaId",
                table: "AccionesDeModeracion",
                column: "DenunciaId",
                principalTable: "Denuncias",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccionesDeModeracion_Denuncias_DenunciaId",
                table: "AccionesDeModeracion");

            migrationBuilder.AddForeignKey(
                name: "FK_AccionesDeModeracion_Denuncias_DenunciaId",
                table: "AccionesDeModeracion",
                column: "DenunciaId",
                principalTable: "Denuncias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
