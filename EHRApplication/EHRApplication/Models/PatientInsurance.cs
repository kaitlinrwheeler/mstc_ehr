using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientInsurance
    {
        [Key]
        public int patientInsuranceId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        
        public int MHN { get; set; }

        [MaxLength(100, ErrorMessage = "Please enter between 1 and 100 characters.")]
        [Required(ErrorMessage = "Provider name is required")]
        public string providerName { get; set; }

        [MaxLength(100, ErrorMessage = "Please enter between 1 and 100 characters.")]
        [Required(ErrorMessage = "Member ID is required")]
        public string memberId { get; set; }

        [MaxLength(100, ErrorMessage = "Please enter between 1 and 100 characters.")]
        [Required(ErrorMessage = "Policy number is required")]
        public string policyNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Please enter between 1 and 100 characters.")]
        [Required(ErrorMessage = "Group number is required")]
        public string groupNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Please enter between 1 and 100 characters.")]
        [Required(ErrorMessage = "Priority is required")]
        public string priority { get; set; }

        [ForeignKey("primaryPhysician")]
        public Providers providers { get; set; }

        [Required(ErrorMessage = "Primary physician is required")]
        public int primaryPhysician {  get; set; }

        public bool active { get; set; }
    }
}
