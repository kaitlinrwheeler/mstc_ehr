using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class Vitals
    {
        [Key]
        public int vitalsId { get; set; }

        public Visits visits { get; set; }

        public int visitId { get; set; }

        public PatientDemographic patients { get; set; }

        public int patientId { get; set; }

        public decimal temperature { get; set; }

        public int systolicPressure { get; set; }

        public int diastolicPressure { get; set; }

        public int heartRate { get; set; }

        public int respiratoryRate { get; set; }

        public decimal pulseOximetry { get; set; }

        public decimal heightInches { get; set; }

        public decimal weightPounds { get; set; }

        public decimal intakeMilliLiters { get; set; }

        public decimal outputMilliLiters { get; set; }
    }
}
