using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHRApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddHasAlertFieldToPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ethnicity",
                table: "PatientDemographic",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AddColumn<bool>(
                name: "HasAlerts",
                table: "PatientDemographic",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAlerts",
                table: "PatientDemographic");

            migrationBuilder.AlterColumn<string>(
                name: "ethnicity",
                table: "PatientDemographic",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
