using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientDemographic
    {
        [Key]
        public int MHN { get; set; }

        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }

        public string suffix { get; set; }

        public string preferredPronouns { get; set; }

        public DateOnly DOB { get; set; }

        public string gender { get; set; }

        public string preferredLanguage { get; set; }

        public string ethnicity { get; set; }

        public string race { get; set; }

        public string religion { get; set; }

        [ForeignKey("primaryPhysician")]
        public Providers providers { get; set; }

        public int primaryPhysician { get; set; }

        public string legalGuardian1 { get; set; }

        public string legalGuardian2 { get; set; }

        public string previousName { get; set; }

        public string genderAssignedAtBirth { get; set; }
    }
}
