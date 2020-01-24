using Microsoft.EntityFrameworkCore.Migrations;

namespace FileManagerApp.Migrations
{
    public partial class ChangedFileNameFromPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "Files",
                newName: "FilePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Files",
                newName: "PhotoPath");
        }
    }
}
