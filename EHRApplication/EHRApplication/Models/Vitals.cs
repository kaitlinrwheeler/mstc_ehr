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
        [Range(0, 100000, ErrorMessage = "Please select a visit.")]
        public int visitId { get; set; }

        [ForeignKey("patientId")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }

        [Required(ErrorMessage ="you must have a patient be selected.")]
        public int patientId { get; set; }

        [Required(ErrorMessage = "Please enter a pain level.")]
        [Range(0, 5, ErrorMessage = "Please select a pain level.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Pain level must be a number.")]
        public int painLevel { get; set; }

        [Required(ErrorMessage = "Please enter a temperature.")]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Temperature must be between 1 and 100.")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Temperature must be a number.")]
        public decimal temperature { get; set; }

        [Required(ErrorMessage = "Please enter a blood pressure.")]
        [StringLength(10, ErrorMessage = "Blood pressure must not exceed 10 characters.")]
        [RegularExpression(@"^\d+(/\d+)?$", ErrorMessage = "Blood pressure should contain only numbers and '/' character.")]
        public string bloodPressure { get; set; }

        [Required(ErrorMessage = "Please enter a pulse.")]
        [Range(30, 200, ErrorMessage = "Pulse must be between 30 and 200.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Pain level must be a number.")]
        public int pulse { get; set; }

        [Required(ErrorMessage = "Please enter a respiratory rate.")]
        [Range(typeof(int), "10", "60", ErrorMessage = "Respiratory rate must be between 10 and 60.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Pain level must be a number.")]
        public int respiratoryRate { get; set; }

        [Required(ErrorMessage = "Please enter a pulse oximetry.")]
        [Range(typeof(decimal), "50", "100", ErrorMessage = "Pulse oximetry must be between 50 and 100.")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Pulse Oximetry must be a number.")]
        public decimal pulseOximetry { get; set; }

        [Required(ErrorMessage = "Please enter a height in inches.")]
        [Range(typeof(decimal), "12", "100", ErrorMessage = "Height must be between 12 and 100.")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Height must be a number.")]
        public decimal heightInches { get; set; }

        [Required(ErrorMessage = "Please enter a weight in pounds.")]
        [Range(typeof(decimal), "2", "1000", ErrorMessage = "Weight must be between 2 and 1000.")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Weight must be a number.")]
        public decimal weightPounds { get; set; }

        [ValidateNever]
        public decimal BMI { get; set; }

        [Required(ErrorMessage = "Please enter an intake in milliliters.")]
        [Range(typeof(int), "1", "5000", ErrorMessage = "Intake must be between 1 and 5000.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Pain level must be a number.")]
        public int intakeMilliLiters { get; set; }

        [Required(ErrorMessage = "Please enter a output in milliliters.")]
        [Range(typeof(int), "1", "5000", ErrorMessage = "Output must be between 1 and 5000.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Pain level must be a number.")]
        public int outputMilliLiters { get; set; }
    }
}
