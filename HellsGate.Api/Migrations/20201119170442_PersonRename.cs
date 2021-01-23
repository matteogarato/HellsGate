using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HellsGate.Api.Migrations
{
    public partial class PersonRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autorizations_Peoples_PeopleAnagraphicModelId",
                table: "Autorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_SafeAuthModels_Peoples_PeopleAnagraphicModelId",
                table: "SafeAuthModels");

            migrationBuilder.DropIndex(
                name: "IX_SafeAuthModels_PeopleAnagraphicModelId",
                table: "SafeAuthModels");

            migrationBuilder.DropIndex(
                name: "IX_Autorizations_PeopleAnagraphicModelId",
                table: "Autorizations");

            migrationBuilder.DropColumn(
                name: "PeopleAnagraphicModelId",
                table: "SafeAuthModels");

            migrationBuilder.DropColumn(
                name: "PeopleAnagraphicModelId",
                table: "Autorizations");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonModelId",
                table: "SafeAuthModels",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PersonModelId",
                table: "Autorizations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SafeAuthModels_PersonModelId",
                table: "SafeAuthModels",
                column: "PersonModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Autorizations_PersonModelId",
                table: "Autorizations",
                column: "PersonModelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Autorizations_Peoples_PersonModelId",
                table: "Autorizations",
                column: "PersonModelId",
                principalTable: "Peoples",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SafeAuthModels_Peoples_PersonModelId",
                table: "SafeAuthModels",
                column: "PersonModelId",
                principalTable: "Peoples",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autorizations_Peoples_PersonModelId",
                table: "Autorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_SafeAuthModels_Peoples_PersonModelId",
                table: "SafeAuthModels");

            migrationBuilder.DropIndex(
                name: "IX_SafeAuthModels_PersonModelId",
                table: "SafeAuthModels");

            migrationBuilder.DropIndex(
                name: "IX_Autorizations_PersonModelId",
                table: "Autorizations");

            migrationBuilder.DropColumn(
                name: "PersonModelId",
                table: "SafeAuthModels");

            migrationBuilder.DropColumn(
                name: "PersonModelId",
                table: "Autorizations");

            migrationBuilder.AddColumn<Guid>(
                name: "PeopleAnagraphicModelId",
                table: "SafeAuthModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PeopleAnagraphicModelId",
                table: "Autorizations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SafeAuthModels_PeopleAnagraphicModelId",
                table: "SafeAuthModels",
                column: "PeopleAnagraphicModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Autorizations_PeopleAnagraphicModelId",
                table: "Autorizations",
                column: "PeopleAnagraphicModelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Autorizations_Peoples_PeopleAnagraphicModelId",
                table: "Autorizations",
                column: "PeopleAnagraphicModelId",
                principalTable: "Peoples",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SafeAuthModels_Peoples_PeopleAnagraphicModelId",
                table: "SafeAuthModels",
                column: "PeopleAnagraphicModelId",
                principalTable: "Peoples",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}