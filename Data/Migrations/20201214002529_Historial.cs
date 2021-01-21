using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Historial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccionesDeModeracion",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creacion = table.Column<DateTimeOffset>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true),
                    TipoElemento = table.Column<int>(nullable: false),
                    HiloId = table.Column<string>(nullable: true),
                    ComentarioId = table.Column<string>(nullable: true),
                    DenunciaId = table.Column<string>(nullable: true),
                    Nota = table.Column<string>(nullable: true),
                    BanId = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccionesDeModeracion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccionesDeModeracion_Bans_BanId",
                        column: x => x.BanId,
                        principalTable: "Bans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AccionesDeModeracion_Comentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AccionesDeModeracion_Denuncias_DenunciaId",
                        column: x => x.DenunciaId,
                        principalTable: "Denuncias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccionesDeModeracion_Hilos_HiloId",
                        column: x => x.HiloId,
                        principalTable: "Hilos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AccionesDeModeracion_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccionesDeModeracion_BanId",
                table: "AccionesDeModeracion",
                column: "BanId");

            migrationBuilder.CreateIndex(
                name: "IX_AccionesDeModeracion_ComentarioId",
                table: "AccionesDeModeracion",
                column: "ComentarioId");

            migrationBuilder.CreateIndex(
                name: "IX_AccionesDeModeracion_DenunciaId",
                table: "AccionesDeModeracion",
                column: "DenunciaId");

            migrationBuilder.CreateIndex(
                name: "IX_AccionesDeModeracion_HiloId",
                table: "AccionesDeModeracion",
                column: "HiloId");

            migrationBuilder.CreateIndex(
                name: "IX_AccionesDeModeracion_UsuarioId",
                table: "AccionesDeModeracion",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccionesDeModeracion");
        }
    }
}
