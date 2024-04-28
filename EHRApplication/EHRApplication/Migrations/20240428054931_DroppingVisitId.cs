using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHRApplication.Migrations
{
    /// <inheritdoc />
    public partial class DroppingVisitId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabResults_Visits_visitId",
                table: "LabResults");

            migrationBuilder.DropIndex(
                name: "IX_LabResults_visitId",
                table: "LabResults");

            migrationBuilder.DropColumn(
                name: "visitId",
                table: "LabResults");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_visitsId",
                table: "LabResults",
                column: "visitsId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResults_Visits_visitsId",
                table: "LabResults",
                column: "visitsId",
                principalTable: "Visits",
                principalColumn: "visitsId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabResults_Visits_visitsId",
                table: "LabResults");

            migrationBuilder.DropIndex(
                name: "IX_LabResults_visitsId",
                table: "LabResults");

            migrationBuilder.AddColumn<int>(
                name: "visitId",
                table: "LabResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_visitId",
                table: "LabResults",
                column: "visitId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResults_Visits_visitId",
                table: "LabResults",
                column: "visitId",
                principalTable: "Visits",
                principalColumn: "visitsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
