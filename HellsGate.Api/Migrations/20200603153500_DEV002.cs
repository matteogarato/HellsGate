using Microsoft.EntityFrameworkCore.Migrations;

namespace HellsGate.Api.Migrations
{
    public partial class DEV002 : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "Access");

            migrationBuilder.DropColumn(
                name: "NodeName",
                table: "Access");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "Access",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NodeName",
                table: "Access",
                nullable: true);
        }
    }
}