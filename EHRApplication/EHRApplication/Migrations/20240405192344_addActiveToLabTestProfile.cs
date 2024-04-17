using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHRApplication.Migrations
{
    /// <inheritdoc />
    public partial class addActiveToLabTestProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherGender",
                table: "PatientDemographic");

            migrationBuilder.DropColumn(
                name: "OtherPronouns",
                table: "PatientDemographic");

            migrationBuilder.DropColumn(
                name: "OtherRace",
                table: "PatientDemographic");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "LabTestProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "LabTestProfile");

            migrationBuilder.AddColumn<string>(
                name: "OtherGender",
                table: "PatientDemographic",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherPronouns",
                table: "PatientDemographic",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherRace",
                table: "PatientDemographic",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);
        }
    }
}
