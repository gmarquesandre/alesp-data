using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alesp.Api.Migrations
{
    public partial class addcongresspersonimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CongressPeople",
                newName: "PictureBase64");

            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "CongressPeople",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biography",
                table: "CongressPeople");

            migrationBuilder.RenameColumn(
                name: "PictureBase64",
                table: "CongressPeople",
                newName: "Description");
        }
    }
}
