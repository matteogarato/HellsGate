using Microsoft.EntityFrameworkCore.Migrations;

namespace HellsGate.Migrations
{
    public partial class Dev003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentMenu",
                table: "MainMenu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentMenu",
                table: "MainMenu");
        }
    }
}
