using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHRApplication.Migrations
{
    /// <inheritdoc />
    public partial class addmigrationUpdateIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientDemographic_Providers_providerId",
                table: "PatientDemographic");

            migrationBuilder.DropTable(
                name: "PatientDx");

            migrationBuilder.RenameColumn(
                name: "providerId",
                table: "PatientDemographic",
                newName: "primaryPhysician");

            migrationBuilder.RenameColumn(
                name: "perferredLanguage",
                table: "PatientDemographic",
                newName: "race");

            migrationBuilder.RenameIndex(
                name: "IX_PatientDemographic_providerId",
                table: "PatientDemographic",
                newName: "IX_PatientDemographic_primaryPhysician");

            migrationBuilder.RenameColumn(
                name: "textName",
                table: "LabTestProfile",
                newName: "testName");

            migrationBuilder.AddColumn<string>(
                name: "genderAssingedAtBirth",
                table: "PatientDemographic",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "preferredLanguage",
                table: "PatientDemographic",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "preferredPronouns",
                table: "PatientDemographic",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "previousName",
                table: "PatientDemographic",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "startDate",
                table: "Alerts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDemographic_Providers_primaryPhysician",
                table: "PatientDemographic",
                column: "primaryPhysician",
                principalTable: "Providers",
                principalColumn: "providerId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientDemographic_Providers_primaryPhysician",
                table: "PatientDemographic");

            migrationBuilder.DropColumn(
                name: "genderAssingedAtBirth",
                table: "PatientDemographic");

            migrationBuilder.DropColumn(
                name: "preferredLanguage",
                table: "PatientDemographic");

            migrationBuilder.DropColumn(
                name: "preferredPronouns",
                table: "PatientDemographic");

            migrationBuilder.DropColumn(
                name: "previousName",
                table: "PatientDemographic");

            migrationBuilder.DropColumn(
                name: "startDate",
                table: "Alerts");

            migrationBuilder.RenameColumn(
                name: "race",
                table: "PatientDemographic",
                newName: "perferredLanguage");

            migrationBuilder.RenameColumn(
                name: "primaryPhysician",
                table: "PatientDemographic",
                newName: "providerId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientDemographic_primaryPhysician",
                table: "PatientDemographic",
                newName: "IX_PatientDemographic_providerId");

            migrationBuilder.RenameColumn(
                name: "testName",
                table: "LabTestProfile",
                newName: "textName");

            migrationBuilder.CreateTable(
                name: "PatientDx",
                columns: table => new
                {
                    patientDxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdBy = table.Column<int>(type: "int", nullable: false),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    Dx = table.Column<string>(type: "varchar(100)", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDx", x => x.patientDxId);
                    table.ForeignKey(
                        name: "FK_PatientDx_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientDx_Providers_createdBy",
                        column: x => x.createdBy,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientDx_createdBy",
                table: "PatientDx",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDx_MHN",
                table: "PatientDx",
                column: "MHN");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDemographic_Providers_providerId",
                table: "PatientDemographic",
                column: "providerId",
                principalTable: "Providers",
                principalColumn: "providerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
