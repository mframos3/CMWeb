using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CMWeb.Data.Migrations
{
    public partial class MealUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Menu_MenuId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_MenuId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Event");
            
            migrationBuilder.CreateTable(
                name: "MealMenu",
                columns: table => new
                {
                    MealId = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealMenu", x => new { x.MealId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_MealMenu_Event_MealId",
                        column: x => x.MealId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealMenu_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            

            migrationBuilder.CreateIndex(
                name: "IX_MealMenu_MenuId",
                table: "MealMenu",
                column: "MenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "MealMenu");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Event",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_MenuId",
                table: "Event",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Menu_MenuId",
                table: "Event",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
