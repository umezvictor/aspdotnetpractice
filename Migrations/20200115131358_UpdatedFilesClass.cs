using Microsoft.EntityFrameworkCore.Migrations;

namespace FileManagerApp.Migrations
{
    public partial class UpdatedFilesClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "Files",
                newName: "PhotoPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "Files",
                newName: "Photo");
        }
    }
}
