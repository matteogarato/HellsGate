using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HellsGate.Migrations
{
    public partial class Dev001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessTime = table.Column<DateTime>(nullable: false),
                    GrantedAccess = table.Column<bool>(nullable: false),
                    Plate = table.Column<string>(nullable: true),
                    PeopleEntered = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardModels",
                columns: table => new
                {
                    CardNumber = table.Column<string>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardModels", x => x.CardNumber);
                });

            migrationBuilder.CreateTable(
                name: "MainMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    AuthLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Peoples",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    CardNumber1 = table.Column<string>(nullable: true),
                    LastModify = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peoples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peoples_CardModels_CardNumber1",
                        column: x => x.CardNumber1,
                        principalTable: "CardModels",
                        principalColumn: "CardNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Autorizations",
                columns: table => new
                {
                    PeopleAnagraphicModelId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    AuthName = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    AuthValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autorizations", x => x.PeopleAnagraphicModelId);
                    table.ForeignKey(
                        name: "FK_Autorizations_Peoples_PeopleAnagraphicModelId",
                        column: x => x.PeopleAnagraphicModelId,
                        principalTable: "Peoples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    LicencePlate = table.Column<string>(nullable: false),
                    Model = table.Column<string>(nullable: true),
                    Colour = table.Column<string>(nullable: true),
                    LastModify = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true)
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
                    PeopleAnagraphicModelId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    AutId = table.Column<int>(nullable: false),
                    Control = table.Column<string>(nullable: true),
                    DtIns = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafeAuthModels", x => x.PeopleAnagraphicModelId);
                    table.ForeignKey(
                        name: "FK_SafeAuthModels_Peoples_PeopleAnagraphicModelId",
                        column: x => x.PeopleAnagraphicModelId,
                        principalTable: "Peoples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Peoples",
                columns: new[] { "Id", "AccessFailedCount", "CardNumber1", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastModify", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0885da9b-ba4f-4698-9419-ff8eb8f9d3ec", 0, null, "1a8baee8-a724-4529-b8d2-8f200e3418a4", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, null, null, "ZmbpVrIuW/wiGze/tuyOaUCrA+onxN5OaHtuKANmccGLvETB", null, null, false, null, null, false, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_OwnerId",
                table: "Cars",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Peoples_CardNumber1",
                table: "Peoples",
                column: "CardNumber1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Access");

            migrationBuilder.DropTable(
                name: "Autorizations");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "MainMenu");

            migrationBuilder.DropTable(
                name: "SafeAuthModels");

            migrationBuilder.DropTable(
                name: "Peoples");

            migrationBuilder.DropTable(
                name: "CardModels");
        }
    }
}