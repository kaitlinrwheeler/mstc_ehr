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

        [Required(ErrorMessage ="Please select a provider.")]
        public int providerId { get; set; }

        public DateOnly date { get; set; }

        public TimeOnly time { get; set; }

        public bool admitted { get; set; }

        [StringLength(70, ErrorMessage ="Visit note must not exceed 70 characters.", MinimumLength = 5)]
        [Required(ErrorMessage ="Please enter a note for the visit.")]
        public string notes { get; set; }
    }
}
