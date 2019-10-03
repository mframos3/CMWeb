using Microsoft.EntityFrameworkCore.Migrations;

namespace CMWeb.Data.Migrations
{
    public partial class NewDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatManager");

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Menu",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Event",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Menu",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Event");

            migrationBuilder.CreateTable(
                name: "StatManager",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatManager", x => x.Id);
                });
        }
    }
}
