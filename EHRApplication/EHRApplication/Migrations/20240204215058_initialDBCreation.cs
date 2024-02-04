using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHRApplication.Migrations
{
    /// <inheritdoc />
    public partial class initialDBCreation : Migration
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
                    allergyType = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.allergyId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(450)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(100)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "varchar(100)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(100)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(100)", nullable: true),
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
                name: "LabTestProfile",
                columns: table => new
                {
                    testId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    textName = table.Column<string>(type: "varchar(100)", nullable: false),
                    description = table.Column<string>(type: "varchar(100)", nullable: false),
                    units = table.Column<string>(type: "varchar(100)", nullable: false),
                    referenceRange = table.Column<string>(type: "varchar(100)", nullable: false),
                    category = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestProfile", x => x.testId);
                });

            migrationBuilder.CreateTable(
                name: "MedicationProfile",
                columns: table => new
                {
                    medId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medName = table.Column<string>(type: "varchar(100)", nullable: false),
                    description = table.Column<string>(type: "varchar(100)", nullable: false),
                    route = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationProfile", x => x.medId);
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
                    RoleId = table.Column<string>(type: "varchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(100)", nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(100)", nullable: true)
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
                    UserId = table.Column<string>(type: "varchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(100)", nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(100)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "varchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "varchar(100)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "varchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "varchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(450)", nullable: false),
                    Name = table.Column<string>(type: "varchar(450)", nullable: false),
                    Value = table.Column<string>(type: "varchar(100)", nullable: true)
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
                    suffix = table.Column<string>(type: "varchar(100)", nullable: false),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "varchar(100)", nullable: false),
                    perferredLanguage = table.Column<string>(type: "varchar(100)", nullable: false),
                    ethnicity = table.Column<string>(type: "varchar(100)", nullable: false),
                    religion = table.Column<string>(type: "varchar(100)", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbdomenCoccyxGenitalia",
                columns: table => new
                {
                    ACGId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    evaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    evaluationTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    abdominalShapeAppearance = table.Column<string>(type: "varchar(100)", nullable: false),
                    bowelSounds = table.Column<string>(type: "varchar(100)", nullable: false),
                    tendernessLumpDistention = table.Column<string>(type: "varchar(100)", nullable: false),
                    tubeDrainCath = table.Column<string>(type: "varchar(100)", nullable: false),
                    skinAppearance = table.Column<string>(type: "varchar(100)", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbdomenCoccyxGenitalia", x => x.ACGId);
                    table.ForeignKey(
                        name: "FK_AbdomenCoccyxGenitalia_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbdomenCoccyxGenitalia_Providers_providerId",
                        column: x => x.providerId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    alertId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    alertName = table.Column<string>(type: "varchar(100)", nullable: false),
                    activeStatus = table.Column<string>(type: "varchar(100)", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "Cardiothoracic",
                columns: table => new
                {
                    cardioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    evaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    evaluationTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    heartSounds = table.Column<string>(type: "varchar(100)", nullable: false),
                    heartRhythm = table.Column<string>(type: "varchar(100)", nullable: false),
                    heartRate = table.Column<string>(type: "varchar(100)", nullable: false),
                    jugularVenousPulse = table.Column<string>(type: "varchar(100)", nullable: false),
                    drainLineSutureStaple = table.Column<string>(type: "varchar(100)", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardiothoracic", x => x.cardioId);
                    table.ForeignKey(
                        name: "FK_Cardiothoracic_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Cardiothoracic_Providers_providerId",
                        column: x => x.providerId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CarePlan",
                columns: table => new
                {
                    CPId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    priority = table.Column<string>(type: "varchar(100)", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    activeStatus = table.Column<string>(type: "varchar(100)", nullable: false),
                    title = table.Column<string>(type: "varchar(100)", nullable: false),
                    diagnosis = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarePlan", x => x.CPId);
                    table.ForeignKey(
                        name: "FK_CarePlan_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ConsciousnessAndOrientation",
                columns: table => new
                {
                    COId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    evaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    evaluationTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    person = table.Column<string>(type: "varchar(100)", nullable: false),
                    place = table.Column<string>(type: "varchar(100)", nullable: false),
                    time = table.Column<string>(type: "varchar(100)", nullable: false),
                    purpose = table.Column<string>(type: "varchar(100)", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsciousnessAndOrientation", x => x.COId);
                    table.ForeignKey(
                        name: "FK_ConsciousnessAndOrientation_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ConsciousnessAndOrientation_Providers_providerId",
                        column: x => x.providerId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Extremities",
                columns: table => new
                {
                    exId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    evaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    evaluationTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    colorTempCapillaryRefill = table.Column<string>(type: "varchar(100)", nullable: false),
                    pulseSensation = table.Column<string>(type: "varchar(100)", nullable: false),
                    edema = table.Column<string>(type: "varchar(100)", nullable: false),
                    rangeOfMotion = table.Column<string>(type: "varchar(100)", nullable: false),
                    tubeDrainSutureStapleCath = table.Column<string>(type: "varchar(100)", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extremities", x => x.exId);
                    table.ForeignKey(
                        name: "FK_Extremities_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Extremities_Providers_providerId",
                        column: x => x.providerId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "General",
                columns: table => new
                {
                    genId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    evaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    evaluationTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    plainDiscomfort = table.Column<string>(type: "varchar(100)", nullable: false),
                    elimination = table.Column<string>(type: "varchar(100)", nullable: false),
                    appetite = table.Column<string>(type: "varchar(100)", nullable: false),
                    activity = table.Column<string>(type: "varchar(100)", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_General", x => x.genId);
                    table.ForeignKey(
                        name: "FK_General_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_General_Providers_providerId",
                        column: x => x.providerId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "HEENT_Neuros",
                columns: table => new
                {
                    HNId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    evaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    evaluationTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    head = table.Column<string>(type: "varchar(100)", nullable: false),
                    vision = table.Column<string>(type: "varchar(100)", nullable: false),
                    hearing = table.Column<string>(type: "varchar(100)", nullable: false),
                    nose = table.Column<string>(type: "varchar(100)", nullable: false),
                    throatMouth = table.Column<string>(type: "varchar(100)", nullable: false),
                    providersId = table.Column<int>(type: "int", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HEENT_Neuros", x => x.HNId);
                    table.ForeignKey(
                        name: "FK_HEENT_Neuros_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HEENT_Neuros_Providers_providersId",
                        column: x => x.providersId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MedAdministrationHistory",
                columns: table => new
                {
                    administrationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    category = table.Column<string>(type: "varchar(100)", nullable: false),
                    medId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(100)", nullable: false),
                    frequency = table.Column<string>(type: "varchar(100)", nullable: false),
                    dateGiven = table.Column<DateOnly>(type: "date", nullable: false),
                    timeGiven = table.Column<TimeOnly>(type: "time", nullable: false),
                    administeredBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedAdministrationHistory", x => x.administrationId);
                    table.ForeignKey(
                        name: "FK_MedAdministrationHistory_MedicationProfile_medId",
                        column: x => x.medId,
                        principalTable: "MedicationProfile",
                        principalColumn: "medId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MedAdministrationHistory_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MedAdministrationHistory_Providers_administeredBy",
                        column: x => x.administeredBy,
                        principalTable: "Providers",
                        principalColumn: "providerId",
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
                    address = table.Column<string>(type: "varchar(100)", nullable: false),
                    city = table.Column<string>(type: "varchar(100)", nullable: false),
                    state = table.Column<string>(type: "varchar(100)", nullable: false),
                    zipcode = table.Column<int>(type: "int", nullable: false),
                    phone = table.Column<string>(type: "varchar(100)", nullable: false),
                    email = table.Column<string>(type: "varchar(100)", nullable: false),
                    ECFirstName = table.Column<string>(type: "varchar(100)", nullable: false),
                    ECLastName = table.Column<string>(type: "varchar(100)", nullable: false),
                    ECRelationship = table.Column<string>(type: "varchar(100)", nullable: false),
                    ECPhone = table.Column<string>(type: "varchar(100)", nullable: false)
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
                    category = table.Column<string>(type: "varchar(100)", nullable: false),
                    activeStatus = table.Column<string>(type: "varchar(100)", nullable: false),
                    prescrptionInstructions = table.Column<string>(type: "varchar(100)", nullable: false),
                    dosage = table.Column<string>(type: "varchar(100)", nullable: false),
                    route = table.Column<string>(type: "varchar(100)", nullable: false),
                    providedBy = table.Column<int>(type: "int", nullable: false),
                    datePrescribed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientMedications", x => x.patientMedId);
                    table.ForeignKey(
                        name: "FK_PatientMedications_MedicationProfile_medId",
                        column: x => x.medId,
                        principalTable: "MedicationProfile",
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
                    category = table.Column<string>(type: "varchar(100)", nullable: false)
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
                name: "PsychMentalHealth",
                columns: table => new
                {
                    psychId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    evaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    evaluationTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    moodAffect = table.Column<string>(type: "varchar(100)", nullable: false),
                    cognition = table.Column<string>(type: "varchar(100)", nullable: false),
                    thoughtPattern = table.Column<string>(type: "varchar(100)", nullable: false),
                    sleepPattern = table.Column<string>(type: "varchar(100)", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false),
                    providersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsychMentalHealth", x => x.psychId);
                    table.ForeignKey(
                        name: "FK_PsychMentalHealth_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PsychMentalHealth_Providers_providerId",
                        column: x => x.providerId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Respiratory",
                columns: table => new
                {
                    respId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    evaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    evaluationTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    lungSounds = table.Column<string>(type: "varchar(100)", nullable: false),
                    respirationDepth = table.Column<string>(type: "varchar(100)", nullable: false),
                    respirationRate = table.Column<string>(type: "varchar(100)", nullable: false),
                    chestShapeAppearance = table.Column<string>(type: "varchar(100)", nullable: false),
                    drainLineSutureStaple = table.Column<string>(type: "varchar(100)", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respiratory", x => x.respId);
                    table.ForeignKey(
                        name: "FK_Respiratory_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Respiratory_Providers_providerId",
                        column: x => x.providerId,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Skin",
                columns: table => new
                {
                    skinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    evaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    evaluationTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    woundsLessions = table.Column<string>(type: "varchar(100)", nullable: false),
                    rednessIrritation = table.Column<string>(type: "varchar(100)", nullable: false),
                    drynessIrritation = table.Column<string>(type: "varchar(100)", nullable: false),
                    colorTemp = table.Column<string>(type: "varchar(100)", nullable: false),
                    signOfBreakdown = table.Column<string>(type: "varchar(100)", nullable: false),
                    providersId = table.Column<int>(type: "int", nullable: false),
                    providerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skin", x => x.skinId);
                    table.ForeignKey(
                        name: "FK_Skin_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Skin_Providers_providersId",
                        column: x => x.providersId,
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
                name: "LabOrders",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    testId = table.Column<int>(type: "int", nullable: false),
                    visitsId = table.Column<int>(type: "int", nullable: false),
                    completionStatus = table.Column<string>(type: "varchar(100)", nullable: false),
                    orderDate = table.Column<DateOnly>(type: "date", nullable: false),
                    orderTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    orderedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabOrders", x => x.orderId);
                    table.ForeignKey(
                        name: "FK_LabOrders_LabTestProfile_testId",
                        column: x => x.testId,
                        principalTable: "LabTestProfile",
                        principalColumn: "testId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LabOrders_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LabOrders_Providers_orderedBy",
                        column: x => x.orderedBy,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LabOrders_Visits_visitsId",
                        column: x => x.visitsId,
                        principalTable: "Visits",
                        principalColumn: "visitsId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LabResults",
                columns: table => new
                {
                    labId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    visitId = table.Column<int>(type: "int", nullable: false),
                    visitsId = table.Column<int>(type: "int", nullable: false),
                    testId = table.Column<int>(type: "int", nullable: false),
                    resultValue = table.Column<string>(type: "varchar(100)", nullable: false),
                    abnormalFlag = table.Column<string>(type: "varchar(100)", nullable: false),
                    orderedBy = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    time = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabResults", x => x.labId);
                    table.ForeignKey(
                        name: "FK_LabResults_LabTestProfile_testId",
                        column: x => x.testId,
                        principalTable: "LabTestProfile",
                        principalColumn: "testId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LabResults_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LabResults_Providers_orderedBy",
                        column: x => x.orderedBy,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LabResults_Visits_visitId",
                        column: x => x.visitId,
                        principalTable: "Visits",
                        principalColumn: "visitsId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MedOrders",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MHN = table.Column<int>(type: "int", nullable: false),
                    visitId = table.Column<int>(type: "int", nullable: false),
                    medId = table.Column<int>(type: "int", nullable: false),
                    frequency = table.Column<string>(type: "varchar(100)", nullable: false),
                    fulfillmentStatus = table.Column<string>(type: "varchar(100)", nullable: false),
                    orderDate = table.Column<DateOnly>(type: "date", nullable: false),
                    orderTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    orderedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedOrders", x => x.orderId);
                    table.ForeignKey(
                        name: "FK_MedOrders_MedicationProfile_medId",
                        column: x => x.medId,
                        principalTable: "MedicationProfile",
                        principalColumn: "medId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MedOrders_PatientDemographic_MHN",
                        column: x => x.MHN,
                        principalTable: "PatientDemographic",
                        principalColumn: "MHN",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MedOrders_Providers_orderedBy",
                        column: x => x.orderedBy,
                        principalTable: "Providers",
                        principalColumn: "providerId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MedOrders_Visits_visitId",
                        column: x => x.visitId,
                        principalTable: "Visits",
                        principalColumn: "visitsId",
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
                    painLevel = table.Column<int>(type: "int", nullable: false),
                    temperature = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    bloodPressure = table.Column<int>(type: "int", nullable: false),
                    diastolicPressure = table.Column<int>(type: "int", nullable: false),
                    respiratoryRate = table.Column<int>(type: "int", nullable: false),
                    pulseOximetry = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    heightInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    weightPounds = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BMI = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbdomenCoccyxGenitalia_MHN",
                table: "AbdomenCoccyxGenitalia",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_AbdomenCoccyxGenitalia_providerId",
                table: "AbdomenCoccyxGenitalia",
                column: "providerId");

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
                name: "IX_Cardiothoracic_MHN",
                table: "Cardiothoracic",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_Cardiothoracic_providerId",
                table: "Cardiothoracic",
                column: "providerId");

            migrationBuilder.CreateIndex(
                name: "IX_CarePlan_MHN",
                table: "CarePlan",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_ConsciousnessAndOrientation_MHN",
                table: "ConsciousnessAndOrientation",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_ConsciousnessAndOrientation_providerId",
                table: "ConsciousnessAndOrientation",
                column: "providerId");

            migrationBuilder.CreateIndex(
                name: "IX_Extremities_MHN",
                table: "Extremities",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_Extremities_providerId",
                table: "Extremities",
                column: "providerId");

            migrationBuilder.CreateIndex(
                name: "IX_General_MHN",
                table: "General",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_General_providerId",
                table: "General",
                column: "providerId");

            migrationBuilder.CreateIndex(
                name: "IX_HEENT_Neuros_MHN",
                table: "HEENT_Neuros",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_HEENT_Neuros_providersId",
                table: "HEENT_Neuros",
                column: "providersId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrders_MHN",
                table: "LabOrders",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrders_orderedBy",
                table: "LabOrders",
                column: "orderedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrders_testId",
                table: "LabOrders",
                column: "testId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrders_visitsId",
                table: "LabOrders",
                column: "visitsId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_MHN",
                table: "LabResults",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_orderedBy",
                table: "LabResults",
                column: "orderedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_testId",
                table: "LabResults",
                column: "testId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_visitId",
                table: "LabResults",
                column: "visitId");

            migrationBuilder.CreateIndex(
                name: "IX_MedAdministrationHistory_administeredBy",
                table: "MedAdministrationHistory",
                column: "administeredBy");

            migrationBuilder.CreateIndex(
                name: "IX_MedAdministrationHistory_medId",
                table: "MedAdministrationHistory",
                column: "medId");

            migrationBuilder.CreateIndex(
                name: "IX_MedAdministrationHistory_MHN",
                table: "MedAdministrationHistory",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_MedOrders_medId",
                table: "MedOrders",
                column: "medId");

            migrationBuilder.CreateIndex(
                name: "IX_MedOrders_MHN",
                table: "MedOrders",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_MedOrders_orderedBy",
                table: "MedOrders",
                column: "orderedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MedOrders_visitId",
                table: "MedOrders",
                column: "visitId");

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
                name: "IX_PsychMentalHealth_MHN",
                table: "PsychMentalHealth",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_PsychMentalHealth_providerId",
                table: "PsychMentalHealth",
                column: "providerId");

            migrationBuilder.CreateIndex(
                name: "IX_Respiratory_MHN",
                table: "Respiratory",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_Respiratory_providerId",
                table: "Respiratory",
                column: "providerId");

            migrationBuilder.CreateIndex(
                name: "IX_Skin_MHN",
                table: "Skin",
                column: "MHN");

            migrationBuilder.CreateIndex(
                name: "IX_Skin_providersId",
                table: "Skin",
                column: "providersId");

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
                name: "AbdomenCoccyxGenitalia");

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
                name: "Cardiothoracic");

            migrationBuilder.DropTable(
                name: "CarePlan");

            migrationBuilder.DropTable(
                name: "ConsciousnessAndOrientation");

            migrationBuilder.DropTable(
                name: "Extremities");

            migrationBuilder.DropTable(
                name: "General");

            migrationBuilder.DropTable(
                name: "HEENT_Neuros");

            migrationBuilder.DropTable(
                name: "LabOrders");

            migrationBuilder.DropTable(
                name: "LabResults");

            migrationBuilder.DropTable(
                name: "MedAdministrationHistory");

            migrationBuilder.DropTable(
                name: "MedOrders");

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
                name: "PsychMentalHealth");

            migrationBuilder.DropTable(
                name: "Respiratory");

            migrationBuilder.DropTable(
                name: "Skin");

            migrationBuilder.DropTable(
                name: "Vitals");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LabTestProfile");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "MedicationProfile");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "PatientDemographic");

            migrationBuilder.DropTable(
                name: "Providers");
        }
    }
}
