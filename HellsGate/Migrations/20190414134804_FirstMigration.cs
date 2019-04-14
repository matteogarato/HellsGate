using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HellsGate.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccessTime = table.Column<DateTime>(nullable: false),
                    GrantedAccess = table.Column<bool>(nullable: false),
                    Plate = table.Column<string>(nullable: true),
                    PeopleEntered = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Peoples",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    LastModify = table.Column<DateTime>(nullable: false),
                    AutorizationLevel_AuthName = table.Column<string>(nullable: true),
                    AutorizationLevel_CreationDate = table.Column<DateTime>(nullable: false),
                    AutorizationLevel_Vaidity = table.Column<TimeSpan>(nullable: false),
                    AutorizationLevel_AuthValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peoples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    LicencePlate = table.Column<string>(nullable: false),
                    LastModify = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.LicencePlate);
                    table.ForeignKey(
                        name: "FK_Cars_Peoples_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Peoples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SafeAuthModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserSafe = table.Column<byte[]>(nullable: true),
                    AutSafe = table.Column<byte[]>(nullable: true),
                    DtIns = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafeAuthModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafeAuthModels_Peoples_Id",
                        column: x => x.Id,
                        principalTable: "Peoples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_OwnerId",
                table: "Cars",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Access");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "SafeAuthModels");

            migrationBuilder.DropTable(
                name: "Peoples");
        }
    }
}
