using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class Vitals
    {
        [Key]
        public int vitalsId { get; set; }

        [ForeignKey("visitId")]
        [ValidateNever]
        public Visits visits { get; set; }

        [Required(ErrorMessage ="You must select a visit")]
        public int visitId { get; set; }

        [ForeignKey("patientId")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }

        public int patientId { get; set; }

        public int painLevel { get; set; }

        public decimal temperature { get; set; }

        [Required(ErrorMessage = "Please enter a blood pressure.")]
        [StringLength(10, ErrorMessage = "Blood pressure must not exceed 10 characters.")]
        public string bloodPressure { get; set; }

        public int pulse { get; set; }

        public int respiratoryRate { get; set; }

        public int pulseOximetry { get; set; }

        public decimal heightInches { get; set; }

        public decimal weightPounds { get; set; }

        [ValidateNever]
        public decimal BMI { get; set; }

        public int intakeMilliLiters { get; set; }

        public int outputMilliLiters { get; set; }
    }
}
