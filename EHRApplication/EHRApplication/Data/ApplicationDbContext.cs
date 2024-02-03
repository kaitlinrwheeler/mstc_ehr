using EHRApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EHRApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Alerts> Alerts { get; set; }

        public DbSet<Allergies> Allergies { get; set; }

        public DbSet<Medications> Medications { get; set; }

        public DbSet<PatientAllergies> PatientAllergies { get; set; }

        public DbSet<PatientContact> PatientContact { get; set; }

        public DbSet<PatientDemographic> PatientDemographic { get; set; }

        public DbSet<PatientDx> PatientDx { get; set; }

        public DbSet<PatientInsurance> PatientInsurance { get; set; }

        public DbSet<PatientMedications> PatientMedications { get; set; }

        public DbSet<PatientNotes> PatientNotes { get; set; }

        public DbSet<Providers> Providers { get; set; }

        public DbSet<Visits> Visits { get; set; }

        public DbSet<Vitals> Vitals { get; set; }
    }
}
