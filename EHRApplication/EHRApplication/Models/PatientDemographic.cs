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

        public DateOnly DOB { get; set; }

        public string gender { get; set; }

        public string perferredLanguage { get; set; }

        public string ethnicity { get; set; }

        public string religion { get; set; }

        [ForeignKey("providerId")]
        public Providers providers { get; set; }

        public int  providerId { get; set; }

        public string legalGuardian1 { get; set; }

        public string legalGuardian2 { get; set; }
    }
}
