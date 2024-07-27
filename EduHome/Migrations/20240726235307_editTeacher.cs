using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.Migrations
{
    public partial class editTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PinterestLink",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TwitterLink",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VKLink",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookLink",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PinterestLink",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "TwitterLink",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "VKLink",
                table: "Teachers");
        }
    }
}
