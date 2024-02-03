using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientAllergies
    {
        [Key]
        public int patientAllergyId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("allergyId")]
        public Allergies allergies { get; set; }

        public int allergyId { get; set; }

        public DateOnly onSetDate { get; set; }
    }
}
