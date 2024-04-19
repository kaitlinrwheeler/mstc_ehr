using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class Visits
    {
        [Key]
        public int visitsId { get; set; }

        [ForeignKey("MHN")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("providerId")]
        [ValidateNever]
        public Providers providers { get; set; }

        [Required(ErrorMessage = "Please select a provider.")]
        public int providerId { get; set; }
        [Required]
        public DateOnly date { get; set; }
        [Required]
        public TimeOnly time { get; set; }

        [Required(ErrorMessage = "Please select one.")]
        public bool admitted { get; set; }

        [StringLength(70, ErrorMessage ="Visit note must not exceed 70 characters.")]
        [Required(ErrorMessage ="Please enter a note for the visit.")]
        public string notes { get; set; }
    }
}
