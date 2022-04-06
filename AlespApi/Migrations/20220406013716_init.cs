using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alesp.Api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CongressPeople",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreasOfWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlespId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongressPeople", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Legislatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legislatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identification = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    IdentificationType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShareCapital = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CongressPersonLegislature",
                columns: table => new
                {
                    CongressPeopleId = table.Column<int>(type: "int", nullable: false),
                    LegislaturesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongressPersonLegislature", x => new { x.CongressPeopleId, x.LegislaturesId });
                    table.ForeignKey(
                        name: "FK_CongressPersonLegislature_CongressPeople_CongressPeopleId",
                        column: x => x.CongressPeopleId,
                        principalTable: "CongressPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CongressPersonLegislature_Legislatures_LegislaturesId",
                        column: x => x.LegislaturesId,
                        principalTable: "Legislatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spendings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CongressPersonId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spendings_CongressPeople_CongressPersonId",
                        column: x => x.CongressPersonId,
                        principalTable: "CongressPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Spendings_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CongressPersonLegislature_LegislaturesId",
                table: "CongressPersonLegislature",
                column: "LegislaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Legislatures_Number",
                table: "Legislatures",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spendings_CongressPersonId_Date_Type_ProviderId",
                table: "Spendings",
                columns: new[] { "CongressPersonId", "Date", "Type", "ProviderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spendings_ProviderId",
                table: "Spendings",
                column: "ProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CongressPersonLegislature");

            migrationBuilder.DropTable(
                name: "Spendings");

            migrationBuilder.DropTable(
                name: "Legislatures");

            migrationBuilder.DropTable(
                name: "CongressPeople");

            migrationBuilder.DropTable(
                name: "Providers");
        }
    }
}
