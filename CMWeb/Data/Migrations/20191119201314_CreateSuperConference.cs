using Microsoft.EntityFrameworkCore.Migrations;

namespace CMWeb.Data.Migrations
{
    public partial class CreateSuperConference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Conference",
                newName: "Edition");

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Conference",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Conference");

            migrationBuilder.RenameColumn(
                name: "Edition",
                table: "Conference",
                newName: "Name");
        }
    }
}
