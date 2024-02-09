using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHRApplication.Migrations
{
    /// <inheritdoc />
    public partial class ChangedToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_Providers_providedBy",
                table: "PatientMedications");

            migrationBuilder.DropIndex(
                name: "IX_PatientMedications_providedBy",
                table: "PatientMedications");

            migrationBuilder.DropColumn(
                name: "providedBy",
                table: "PatientMedications");

            migrationBuilder.AlterColumn<string>(
                name: "ICD_10",
                table: "PatientProblems",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.CreateIndex(
              name: "IX_PatientMedications_prescribedBy",
              table: "PatientMedications",
              column: "prescribedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_Providers_prescribedBy",
                table: "PatientMedications",
                column: "prescribedBy",
                principalTable: "Providers",
                principalColumn: "providerId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_Providers_prescribedBy",
                table: "PatientMedications");

            migrationBuilder.DropIndex(
                name: "IX_PatientMedications_prescribedBy",
                table: "PatientMedications");

            migrationBuilder.AlterColumn<string>(
                name: "ICD_10",
                table: "PatientProblems",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_providedBy",
                table: "PatientMedications",
                column: "providedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_Providers_providedBy",
                table: "PatientMedications",
                column: "providedBy",
                principalTable: "Providers",
                principalColumn: "providerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
