using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientMedications
    {
        [Key]
        public int patientMedId { get; set; }

        [ForeignKey("MHN")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }

        [Required]
        public int MHN { get; set; }

        [ForeignKey("medId")]
        [ValidateNever]
        public MedicationProfile medProfile { get; set; }

        [Required(ErrorMessage = "Please select a medication")]
        [Range(0,1000, ErrorMessage = "Please select a medication.")]
        public int medId { get; set; }

        [Required(ErrorMessage = "Please enter something for category.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Please enter only letters for category.")]
        public string category { get; set; }

        [Required(ErrorMessage = "Please select active or not.")]
        public string activeStatus { get; set; }

        [Required(ErrorMessage = "Please enter something for instructions.")]
        public string prescriptionInstructions { get; set; }

        [Required(ErrorMessage = "Please enter something for dosage.")]
        public string dosage { get; set; }

        public string route {  get; set; }

        [ForeignKey("prescribedBy")]
        [ValidateNever]
        public Providers providers { get; set; }

        [Required(ErrorMessage = "Please select a provider.")]
        [Range(0, 1000, ErrorMessage = "Please select a provider.")]
        public int prescribedBy { get; set; }

        [Required(ErrorMessage = "Please enter a date.")]
        public DateTime datePrescribed { get; set; }

        [Required(ErrorMessage = "Please enter a date.")]
        public DateTime endDate { get; set; }
    }
}
