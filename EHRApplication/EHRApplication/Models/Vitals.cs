using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class Vitals
    {
        [Key]
        public int vitalsId { get; set; }

        [ForeignKey("visitId")]
        public Visits visits { get; set; }

        public int visitId { get; set; }

        [ForeignKey("patientId")]
        public PatientDemographic patients { get; set; }

        public int patientId { get; set; }

        public int painLevel { get; set; }

        public decimal temperature { get; set; }

        public int bloodPressure { get; set; }

        public int respiratoryRate { get; set; }

        public int pulseOximetry { get; set; }

        public decimal heightInches { get; set; }

        public decimal weightPounds { get; set; }

        public decimal BMI { get; set; }

        public int intakeMilliLiters { get; set; }

        public int outputMilliLiters { get; set; }
    }
}
