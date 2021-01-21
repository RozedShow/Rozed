using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Data.Migrations
{
    public partial class MigracionInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    Creacion = table.Column<DateTimeOffset>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creacion = table.Column<DateTimeOffset>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: false),
                    TgMedia_UrlTgId = table.Column<string>(nullable: true),
                    TgMedia_VistaPreviaTgId = table.Column<string>(nullable: true),
                    TgMedia_VistaPreviaCuadradoTgId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stickies",
                columns: table => new
                {
                    HiloId = table.Column<string>(nullable: false),
                    Expiracion = table.Column<DateTimeOffset>(nullable: false),
                    Global = table.Column<bool>(nullable: false),
                    Importancia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stickies", x => x.HiloId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hilos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creacion = table.Column<DateTimeOffset>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Rango = table.Column<int>(nullable: false),
                    MediaId = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Titulo = table.Column<string>(nullable: true),
                    Contenido = table.Column<string>(nullable: true),
                    CategoriaId = table.Column<int>(nullable: false),
                    Bump = table.Column<DateTimeOffset>(nullable: false),
                    Estado = table.Column<int>(nullable: false),
                    Flags = table.Column<string>(nullable: true),
                    Encuesta = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hilos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hilos_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hilos_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creacion = table.Column<DateTimeOffset>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Rango = table.Column<int>(nullable: false),
                    MediaId = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    HiloId = table.Column<string>(nullable: false),
                    Contenido = table.Column<string>(nullable: true),
                    Estado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Hilos_HiloId",
                        column: x => x.HiloId,
                        principalTable: "Hilos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentarios_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HiloAcciones",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creacion = table.Column<DateTimeOffset>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true),
                    HiloId = table.Column<string>(nullable: true),
                    Seguido = table.Column<bool>(nullable: false),
                    Favorito = table.Column<bool>(nullable: false),
                    Hideado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiloAcciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HiloAcciones_Hilos_HiloId",
                        column: x => x.HiloId,
                        principalTable: "Hilos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HiloAcciones_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bans",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creacion = table.Column<DateTimeOffset>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true),
                    ModId = table.Column<string>(nullable: true),
                    Expiracion = table.Column<DateTimeOffset>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    HiloId = table.Column<string>(nullable: true),
                    ComentarioId = table.Column<string>(nullable: true),
                    Motivo = table.Column<int>(nullable: false),
                    Aclaracion = table.Column<string>(nullable: true),
                    Visto = table.Column<bool>(nullable: false),
                    Ip = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bans_Comentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bans_Hilos_HiloId",
                        column: x => x.HiloId,
                        principalTable: "Hilos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bans_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Denuncias",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creacion = table.Column<DateTimeOffset>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: false),
                    HiloId = table.Column<string>(nullable: false),
                    ComentarioId = table.Column<string>(nullable: true),
                    Motivo = table.Column<int>(nullable: false),
                    Aclaracion = table.Column<string>(nullable: true),
                    Estado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denuncias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Denuncias_Comentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Denuncias_Hilos_HiloId",
                        column: x => x.HiloId,
                        principalTable: "Hilos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Denuncias_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true),
                    HiloId = table.Column<string>(nullable: true),
                    ComentarioId = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: false),
                    Actualizacion = table.Column<DateTimeOffset>(nullable: false),
                    Conteo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Comentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Hilos_HiloId",
                        column: x => x.HiloId,
                        principalTable: "Hilos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificaciones_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                        onDelete: ReferentialAction.SetNull);
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bans_ComentarioId",
                table: "Bans",
                column: "ComentarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Bans_HiloId",
                table: "Bans",
                column: "HiloId");

            migrationBuilder.CreateIndex(
                name: "IX_Bans_UsuarioId",
                table: "Bans",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_HiloId",
                table: "Comentarios",
                column: "HiloId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_MediaId",
                table: "Comentarios",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UsuarioId",
                table: "Comentarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_ComentarioId",
                table: "Denuncias",
                column: "ComentarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_HiloId",
                table: "Denuncias",
                column: "HiloId");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_UsuarioId",
                table: "Denuncias",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HiloAcciones_HiloId",
                table: "HiloAcciones",
                column: "HiloId");

            migrationBuilder.CreateIndex(
                name: "IX_HiloAcciones_UsuarioId",
                table: "HiloAcciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Hilos_Bump",
                table: "Hilos",
                column: "Bump");

            migrationBuilder.CreateIndex(
                name: "IX_Hilos_MediaId",
                table: "Hilos",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Hilos_UsuarioId",
                table: "Hilos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_ComentarioId",
                table: "Notificaciones",
                column: "ComentarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_HiloId",
                table: "Notificaciones",
                column: "HiloId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioId",
                table: "Notificaciones",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccionesDeModeracion");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HiloAcciones");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "Stickies");

            migrationBuilder.DropTable(
                name: "Bans");

            migrationBuilder.DropTable(
                name: "Denuncias");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Hilos");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
