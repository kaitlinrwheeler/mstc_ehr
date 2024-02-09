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
        public MedicationProfile medProfile { get; set; }

        public int medId { get; set; }

        public string category { get; set; }

        public string activeStatus { get; set; }
        
        public string prescriptionInstructions { get; set; }

        public string dosage { get; set; }

        public string route {  get; set; }

        [ForeignKey("prescribedBy")]
        public Providers providers { get; set; }

        public int prescribedBy { get; set; }

        public DateTime datePrescribed { get; set; }

        public DateTime endDate { get; set; }
    }
}
