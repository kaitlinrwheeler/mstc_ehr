using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHRApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    allergyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    allergyName = table.Column<string>(type: "varchar(100)", nullable: false),
                    allergyType = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.allergyId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    medId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medName = table.Column<string>(type: "varchar(100)", nullable: false),
                    description = table.Column<string>(type: "varchar(500)", nullable: false),
                    sideEffects = table.Column<string>(type: "varchar(500)", nullable: false),
                    manufacturer = table.Column<string>(type: "varchar(100)", nullable: false),
                    route = table.Column<string>(type: "varchar(100)", nullable: false),
                    storageRequirements = table.Column<string>(type: "varchar(500)", nullable: false),
                    contraindications = table.Column<string>(type: "varchar(500)", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    category = table.Column<string>(type: "varchar(100)", nullable: false),
                    homeMeds = table.Column<bool>(type: "bit", nullable: false),
                    author = table.Column<string>(type: "varchar(100)", nullable: false),
                    provider = table.Column<string>(type: "varchar(100)", nullable: false),
                    medication = table.Column<string>(type: "varchar(100)", nullable: false),
                    includeDEA_NPINumber = table.Column<int>(type: "int", nullable: false),
                    alternateName = table.Column<string>(type: "varchar(100)", nullable: false),
                    barcodeId = table.Column<int>(type: "int", nullable: false),
                    orderDetails = table.Column<string>(type: "varchar(500)", nullable: false),
                    frequency = table.Column<string>(type: "varchar(100)", nullable: false),
                    status = table.Column<string>(type: "varchar(100)", nullable: false),
                    startsOn = table.Column<DateOnly>(type: "date", nullable: false),
                    endsOn = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.medId);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    providerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "varchar(100)", nullable: false),
                    lastName = table.Column<string>(type: "varchar(100)", nullable: false),
                    specialty = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.providerId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientDemographic",
                columns: table => new
                {
                    MHN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "varchar(100)", nullable: false),
                    middleName = table.Column<string>(type: "varchar(100)", nullable: false),
                    lastName = table.Column<string>(type: "varchar(100)", nullable: false),
                    suffix = table.Column<string>(type: "varchar(10)", nullable: true),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "varchar(60)", nullable: false),
                    perferredLanguage = table.Column<string>(type: "varchar(60)", nullable: false),
                    ethnicity = table.Column<string>(type: "varchar(100)", nullable: false),
                    religion = table.Column<string>(type: "varchar(60)", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false),
                    legalGuardian1 = table.Column<string>(type: "varchar(100)", nullable: false),
                    legalGuardian2 = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDemographic", x => x.MHN);
                    table.ForeignKey(
                        name: "FK_PatientDemographic_Providers_providerId",
                        column: x => x.providerId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    alertId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    alertName = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.alertId);
                    table.ForeignKey(
                        name: "FK_Alerts_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PatientAllergies",
                columns: table => new
                {
                    patientAllergyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    allergyId = table.Column<int>(type: "int", nullable: false),
                    onSetDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAllergies", x => x.patientAllergyId);
                    table.ForeignKey(
                        name: "FK_PatientAllergies_Allergies_allergyId",
                        column: x => x.allergyId,
                        principalTable: "Allergies",
                        principalColumn: "allergyId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PatientAllergies_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PatientContact",
                columns: table => new
                {
                    patientContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "varchar(150)", nullable: false),
                    city = table.Column<string>(type: "varchar(100)", nullable: false),
                    state = table.Column<string>(type: "varchar(100)", nullable: false),
                    zipcode = table.Column<int>(type: "int", nullable: false),
                    phone = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "varchar(100)", nullable: false),
                    ECFirstName = table.Column<string>(type: "varchar(100)", nullable: false),
                    ECLastName = table.Column<string>(type: "varchar(100)", nullable: false),
                    ECRelationship = table.Column<string>(type: "varchar(100)", nullable: false),
                    ECPhone = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientContact", x => x.patientContactId);
                    table.ForeignKey(
                        name: "FK_PatientContact_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PatientDx",
                columns: table => new
                {
                    patientDxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    Dx = table.Column<string>(type: "varchar(100)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDx", x => x.patientDxId);
                    table.ForeignKey(
                        name: "FK_PatientDx_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PatientDx_Providers_createdBy",
                        column: x => x.createdBy,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PatientInsurance",
                columns: table => new
                {
                    patientInsuranceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    providerName = table.Column<string>(type: "varchar(100)", nullable: false),
                    memberId = table.Column<int>(type: "int", nullable: false),
                    policyNumber = table.Column<int>(type: "int", nullable: false),
                    groupNumber = table.Column<int>(type: "int", nullable: false),
                    priority = table.Column<string>(type: "varchar(100)", nullable: false),
                    primaryPhysician = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInsurance", x => x.patientInsuranceId);
                    table.ForeignKey(
                        name: "FK_PatientInsurance_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PatientInsurance_Providers_primaryPhysician",
                        column: x => x.primaryPhysician,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PatientMedications",
                columns: table => new
                {
                    patientMedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    medId = table.Column<int>(type: "int", nullable: false),
                    providedBy = table.Column<int>(type: "int", nullable: false),
                    datePrescribed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dosage = table.Column<string>(type: "varchar(60)", nullable: false),
                    prescrptionInstructions = table.Column<string>(type: "varchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientMedications", x => x.patientMedId);
                    table.ForeignKey(
                        name: "FK_PatientMedications_Medications_medId",
                        column: x => x.medId,
                        principalTable: "Medications",
                        principalColumn: "medId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PatientMedications_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PatientMedications_Providers_providedBy",
                        column: x => x.providedBy,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PatientNotes",
                columns: table => new
                {
                    patientNotesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "varchar(100)", nullable: false),
                    occurredOn = table.Column<DateOnly>(type: "date", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: false),
                    associatedProvider = table.Column<int>(type: "int", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    category = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientNotes", x => x.patientNotesId);
                    table.ForeignKey(
                        name: "FK_PatientNotes_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PatientNotes_Providers_createdBy",
                        column: x => x.createdBy,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    visitsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    providersId = table.Column<int>(type: "int", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    time = table.Column<TimeOnly>(type: "time", nullable: false),
                    admitted = table.Column<bool>(type: "bit", nullable: false),
                    notes = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.visitsId);
                    table.ForeignKey(
                        name: "FK_Visits_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Visits_Providers_providersId",
                        column: x => x.providersId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Vitals",
                columns: table => new
                {
                    vitalsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    visitId = table.Column<int>(type: "int", nullable: false),
                    patientId = table.Column<int>(type: "int", nullable: false),
                    temperature = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    systolicPressure = table.Column<int>(type: "int", nullable: false),
                    diastolicPressure = table.Column<int>(type: "int", nullable: false),
                    heartRate = table.Column<int>(type: "int", nullable: false),
                    respiratoryRate = table.Column<int>(type: "int", nullable: false),
                    pulseOximetry = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    heightInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    weightPounds = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    intakeMilliLiters = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    outputMilliLiters = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vitals", x => x.vitalsId);
                    table.ForeignKey(
                        name: "FK_Vitals_PatientDemographic_patientId",
                        column: x => x.patientId,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Vitals_Visits_visitId",
                        column: x => x.visitId,
                        principalTable: "Visits",
                        principalColumn: "visitsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_MHN",
                table: "Alerts",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_allergyId",
                table: "PatientAllergies",
                column: "allergyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_MHN",
                table: "PatientAllergies",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_PatientContact_MHN",
                table: "PatientContact",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDemographic_providerId",
                table: "PatientDemographic",
                column: "providerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDx_createdBy",
                table: "PatientDx",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDx_MHN",
                table: "PatientDx",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInsurance_MHN",
                table: "PatientInsurance",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInsurance_primaryPhysician",
                table: "PatientInsurance",
                column: "primaryPhysician");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_medId",
                table: "PatientMedications",
                column: "medId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_MHN",
                table: "PatientMedications",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_providedBy",
                table: "PatientMedications",
                column: "providedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientNotes_createdBy",
                table: "PatientNotes",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientNotes_MHN",
                table: "PatientNotes",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_MHN",
                table: "Visits",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_providersId",
                table: "Visits",
                column: "providersId");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_patientId",
                table: "Vitals",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_visitId",
                table: "Vitals",
                column: "visitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "PatientAllergies");

            migrationBuilder.DropTable(
                name: "PatientContact");

            migrationBuilder.DropTable(
                name: "PatientDx");

            migrationBuilder.DropTable(
                name: "PatientInsurance");

            migrationBuilder.DropTable(
                name: "PatientMedications");

            migrationBuilder.DropTable(
                name: "PatientNotes");

            migrationBuilder.DropTable(
                name: "Vitals");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "PatientDemographic");

            migrationBuilder.DropTable(
                name: "Providers");
        }
    }
}
