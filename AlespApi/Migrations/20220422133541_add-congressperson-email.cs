using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alesp.Api.Migrations
{
    public partial class addcongresspersonemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CongressPeople",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "CongressPeople");
        }
    }
}
