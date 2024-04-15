using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHRApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitIdToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "visitsId",
                table: "PatientProblems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "visitsId",
                table: "PatientNotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "visitsId",
                table: "MedAdministrationHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "visitsId",
                table: "CarePlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientProblems_visitsId",
                table: "PatientProblems",
                column: "visitsId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientNotes_visitsId",
                table: "PatientNotes",
                column: "visitsId");

            migrationBuilder.CreateIndex(
                name: "IX_MedAdministrationHistory_visitsId",
                table: "MedAdministrationHistory",
                column: "visitsId");

            migrationBuilder.CreateIndex(
                name: "IX_CarePlan_visitsId",
                table: "CarePlan",
                column: "visitsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarePlan_Visits_visitsId",
                table: "CarePlan");

            migrationBuilder.DropForeignKey(
                name: "FK_MedAdministrationHistory_Visits_visitsId",
                table: "MedAdministrationHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientNotes_Visits_visitsId",
                table: "PatientNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientProblems_Visits_visitsId",
                table: "PatientProblems");

            migrationBuilder.DropIndex(
                name: "IX_PatientProblems_visitsId",
                table: "PatientProblems");

            migrationBuilder.DropIndex(
                name: "IX_PatientNotes_visitsId",
                table: "PatientNotes");

            migrationBuilder.DropIndex(
                name: "IX_MedAdministrationHistory_visitsId",
                table: "MedAdministrationHistory");

            migrationBuilder.DropIndex(
                name: "IX_CarePlan_visitsId",
                table: "CarePlan");

            migrationBuilder.DropColumn(
                name: "visitsId",
                table: "PatientProblems");

            migrationBuilder.DropColumn(
                name: "visitsId",
                table: "PatientNotes");

            migrationBuilder.DropColumn(
                name: "visitsId",
                table: "MedAdministrationHistory");

            migrationBuilder.DropColumn(
                name: "visitsId",
                table: "CarePlan");
        }
    }
}
