using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CMWeb.Data.Migrations
{
    public partial class SuperConferenceRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SuperConferenceId",
                table: "Conference",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SuperConferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rating = table.Column<float>(nullable: false),
                    IsPeriodic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperConferences", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conference_SuperConferenceId",
                table: "Conference",
                column: "SuperConferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conference_SuperConferences_SuperConferenceId",
                table: "Conference",
                column: "SuperConferenceId",
                principalTable: "SuperConferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conference_SuperConferences_SuperConferenceId",
                table: "Conference");

            migrationBuilder.DropTable(
                name: "SuperConferences");

            migrationBuilder.DropIndex(
                name: "IX_Conference_SuperConferenceId",
                table: "Conference");

            migrationBuilder.DropColumn(
                name: "SuperConferenceId",
                table: "Conference");
        }
    }
}
