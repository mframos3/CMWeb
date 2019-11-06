using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CMWeb.Data.Migrations
{
    public partial class EventsRelationships2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventRating",
                table: "EventRating");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EventRating");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "EventRating",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EventRating",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ConferenceId",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventCenterRoomId",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventRating",
                table: "EventRating",
                columns: new[] { "EventId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_EventRating_UserId",
                table: "EventRating",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ConferenceId",
                table: "Event",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventCenterRoomId",
                table: "Event",
                column: "EventCenterRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Conference_ConferenceId",
                table: "Event",
                column: "ConferenceId",
                principalTable: "Conference",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventCenterRoom_EventCenterRoomId",
                table: "Event",
                column: "EventCenterRoomId",
                principalTable: "EventCenterRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventRating_Event_EventId",
                table: "EventRating",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventRating_AspNetUsers_UserId",
                table: "EventRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Conference_ConferenceId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventCenterRoom_EventCenterRoomId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_EventRating_Event_EventId",
                table: "EventRating");

            migrationBuilder.DropForeignKey(
                name: "FK_EventRating_AspNetUsers_UserId",
                table: "EventRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventRating",
                table: "EventRating");

            migrationBuilder.DropIndex(
                name: "IX_EventRating_UserId",
                table: "EventRating");

            migrationBuilder.DropIndex(
                name: "IX_Event_ConferenceId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_EventCenterRoomId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "EventRating");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EventRating");

            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EventCenterRoomId",
                table: "Event");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EventRating",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventRating",
                table: "EventRating",
                column: "Id");
        }
    }
}
