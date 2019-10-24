using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CMWeb.Migrations
{
    public partial class EventsRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Menu",
                table: "Event");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Event",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventUser",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventUser", x => new { x.EventId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EventUser_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Soup = table.Column<string>(nullable: true),
                    Entree = table.Column<string>(nullable: true),
                    Main = table.Column<string>(nullable: true),
                    Dessert = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_MenuId",
                table: "Event",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_EventUser_UserId",
                table: "EventUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Menu_MenuId",
                table: "Event",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Menu_MenuId",
                table: "Event");

            migrationBuilder.DropTable(
                name: "EventUser");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropIndex(
                name: "IX_Event_MenuId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Event");

            migrationBuilder.AddColumn<string>(
                name: "Menu",
                table: "Event",
                nullable: true);
        }
    }
}
