using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientProblems
    {
        [Key]
        public int patientProblemsId { get; set; }

        [ForeignKey("MHN")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }
        [Required]
        public int MHN { get; set; }

        [ForeignKey("visitsId")]
        [ValidateNever]
        public Visits visits { get; set; }
        [Required(ErrorMessage = "Please select a visit")]
        public int visitsId { get; set; }

        [Required(ErrorMessage = "Please enter priority")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Please enter only letters for priority")]
        public string priority { get; set; }

        [Required(ErrorMessage = "Plese enter a description")]
        public string description { get; set; }

        [Required(ErrorMessage = "Please enter an ICD_10")]
        public string ICD_10 { get; set; }

        [Required(ErrorMessage = "Please enter immediacy")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Please enter only letters for immediacy")]
        public string immediacy { get; set; }

        [Required(ErrorMessage = "Please enter createdAt")]
        public DateTime createdAt { get; set; }

        [ForeignKey("createdBy")]
        [ValidateNever]
        public Providers providers { get; set; }
        [Required(ErrorMessage = "Please select a provider")]
        public int createdBy { get; set; }

        [Required(ErrorMessage = "Plese select active or not")]
        public bool active { get; set; }
    }
}
