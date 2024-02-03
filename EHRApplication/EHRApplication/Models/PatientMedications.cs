using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientMedications
    {
        [Key]
        public int patientMedId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("medId")]
        public Medications medications { get; set; }

        public int medId { get; set; }

        [ForeignKey("providedBy")]
        public Providers providers { get; set; }

        public int providedBy { get; set; }

        public DateTime datePrescribed { get; set; }

        public string dosage { get; set; }

        public string prescrptionInstructions { get; set; }
    }
}
