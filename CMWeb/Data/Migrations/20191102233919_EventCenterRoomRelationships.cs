using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CMWeb.Data.Migrations
{
    public partial class EventCenterRoomRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Borré el drop sponsor

            migrationBuilder.AddColumn<int>(
                name: "EventCenterId",
                table: "EventCenterRoom",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventCenterRoom_EventCenterId",
                table: "EventCenterRoom",
                column: "EventCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCenterRoom_EventCenter_EventCenterId",
                table: "EventCenterRoom",
                column: "EventCenterId",
                principalTable: "EventCenter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCenterRoom_EventCenter_EventCenterId",
                table: "EventCenterRoom");

            migrationBuilder.DropIndex(
                name: "IX_EventCenterRoom_EventCenterId",
                table: "EventCenterRoom");

            migrationBuilder.DropColumn(
                name: "EventCenterId",
                table: "EventCenterRoom");

            migrationBuilder.CreateTable(
                name: "Sponsor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ConferenceId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sponsor_Conference_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sponsor_ConferenceId",
                table: "Sponsor",
                column: "ConferenceId");
        }
    }
}
