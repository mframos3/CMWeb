using Microsoft.EntityFrameworkCore.Migrations;

namespace CMWeb.Data.Migrations
{
    public partial class EditRatingAndConferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Conference");

            migrationBuilder.AddColumn<int>(
                name: "ConferenceId",
                table: "ConferenceRating",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ConferenceRating",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ConferenceRating",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceRating_ConferenceId",
                table: "ConferenceRating",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceRating_UserId1",
                table: "ConferenceRating",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceRating_Conference_ConferenceId",
                table: "ConferenceRating",
                column: "ConferenceId",
                principalTable: "Conference",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceRating_AspNetUsers_UserId1",
                table: "ConferenceRating",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceRating_Conference_ConferenceId",
                table: "ConferenceRating");

            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceRating_AspNetUsers_UserId1",
                table: "ConferenceRating");

            migrationBuilder.DropIndex(
                name: "IX_ConferenceRating_ConferenceId",
                table: "ConferenceRating");

            migrationBuilder.DropIndex(
                name: "IX_ConferenceRating_UserId1",
                table: "ConferenceRating");

            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "ConferenceRating");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ConferenceRating");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ConferenceRating");

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Conference",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
