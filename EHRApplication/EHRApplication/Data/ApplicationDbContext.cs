using EHRApplication.Models;
using EHRApplication.Models.PatientDemographics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EHRApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AbdomenCoccyxGenitalia> AbdomenCoccyxGenitalia { get; set; }

        public DbSet<Alerts> Alerts { get; set; }

        public DbSet<Allergies> Allergies { get; set; }

        public DbSet<ApplicationUser> ApplicationUser {  get; set; }

        public DbSet<Cardiothoracic> Cardiothoracic { get; set; }

        public DbSet<CarePlan> CarePlan { get; set; }

        public DbSet<ConsciousnessAndOrientation> ConsciousnessAndOrientation { get; set; }

        public DbSet<Extremities> Extremities { get; set; }

        public DbSet<General> General { get; set; }

        public DbSet<HEENT_Neuro> HEENT_Neuros { get; set; }

        public DbSet<LabOrders> LabOrders { get; set; }

        public DbSet<LabResults> LabResults { get; set; }

        public DbSet<LabTestProfile> LabTestProfile { get; set; }

        public DbSet<MedAdministrationHistory> MedAdministrationHistory { get; set; }

        public DbSet<MedicationProfile> MedicationProfile { get; set; }

        public DbSet<MedOrders> MedOrders { get; set; }

        public DbSet<PatientAllergies> PatientAllergies { get; set; }

        public DbSet<PatientContact> PatientContact { get; set; }

        public DbSet<PatientDemographic> PatientDemographic { get; set; }

        public DbSet<PatientInsurance> PatientInsurance { get; set; }

        public DbSet<PatientMedications> PatientMedications { get; set; }

        public DbSet<PatientNotes> PatientNotes { get; set; }

        public DbSet<PatientProblems> PatientProblems { get; set; }

        public DbSet<Providers> Providers { get; set; }

        public DbSet<PsychMentalHealth> PsychMentalHealth { get; set; }

        public DbSet<Respiratory> Respiratory { get; set; }

        public DbSet<Skin> Skin { get; set; }

        public DbSet<Visits> Visits { get; set; }

        public DbSet<Vitals> Vitals { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Verification> Verifications { get; set; }
    }
}
