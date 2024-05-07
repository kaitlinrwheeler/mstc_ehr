using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientAllergies
    {
        [Key]
        public int patientAllergyId { get; set; }

        [ForeignKey("MHN")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }

        [Required]
        public int MHN { get; set; }

        [ForeignKey("allergyId")]
        [ValidateNever]
        public Allergies allergies { get; set; }

        [Required(ErrorMessage = "Please select an allergy.")]
        public int allergyId { get; set; }

        [Required(ErrorMessage = "Please enter a date of onset.")]
        public DateOnly onSetDate { get; set; }

        public bool activeStatus { get; set; }
    }
}
