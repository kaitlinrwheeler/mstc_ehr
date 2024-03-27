using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHRApplication.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDecimalsToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "pulseOximetry",
                table: "Vitals",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "outputMilliLiters",
                table: "Vitals",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "intakeMilliLiters",
                table: "Vitals",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<decimal>(
                name: "pulseOximetry",
                table: "Vitals",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "outputMilliLiters",
                table: "Vitals",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "intakeMilliLiters",
                table: "Vitals",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
