using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alesp.Api.Migrations
{
    public partial class addpresence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Presences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CongressPersonId = table.Column<int>(type: "int", nullable: false),
                    LegislativeSessionType = table.Column<int>(type: "int", nullable: false),
                    PresenceStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Presences_CongressPeople_CongressPersonId",
                        column: x => x.CongressPersonId,
                        principalTable: "CongressPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Presences_CongressPersonId",
                table: "Presences",
                column: "CongressPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Presences_Date_CongressPersonId",
                table: "Presences",
                columns: new[] { "Date", "CongressPersonId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Presences");
        }
    }
}
