using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HellsGate.Migrations
{
    public partial class Dev004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DtIns",
                table: "SafeAuthModels");

            migrationBuilder.DropColumn(
                name: "LastModify",
                table: "Cars");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCreated",
                table: "SafeAuthModels",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastCreatedBy",
                table: "SafeAuthModels",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "SafeAuthModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "SafeAuthModels",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCreated",
                table: "MainMenu",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastCreatedBy",
                table: "MainMenu",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "MainMenu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "MainMenu",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCreated",
                table: "Cars",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastCreatedBy",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCreated",
                table: "CardModels",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastCreatedBy",
                table: "CardModels",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "CardModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "CardModels",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCreated",
                table: "Autorizations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastCreatedBy",
                table: "Autorizations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Autorizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Autorizations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCreated",
                table: "Access",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastCreatedBy",
                table: "Access",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Access",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Access",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCreated",
                table: "SafeAuthModels");

            migrationBuilder.DropColumn(
                name: "LastCreatedBy",
                table: "SafeAuthModels");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "SafeAuthModels");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "SafeAuthModels");

            migrationBuilder.DropColumn(
                name: "LastCreated",
                table: "MainMenu");

            migrationBuilder.DropColumn(
                name: "LastCreatedBy",
                table: "MainMenu");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "MainMenu");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "MainMenu");

            migrationBuilder.DropColumn(
                name: "LastCreated",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "LastCreatedBy",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "LastCreated",
                table: "CardModels");

            migrationBuilder.DropColumn(
                name: "LastCreatedBy",
                table: "CardModels");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "CardModels");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "CardModels");

            migrationBuilder.DropColumn(
                name: "LastCreated",
                table: "Autorizations");

            migrationBuilder.DropColumn(
                name: "LastCreatedBy",
                table: "Autorizations");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Autorizations");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Autorizations");

            migrationBuilder.DropColumn(
                name: "LastCreated",
                table: "Access");

            migrationBuilder.DropColumn(
                name: "LastCreatedBy",
                table: "Access");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Access");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Access");

            migrationBuilder.AddColumn<DateTime>(
                name: "DtIns",
                table: "SafeAuthModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModify",
                table: "Cars",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
