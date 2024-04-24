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
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Provider name must contain only alphabetic characters.")]
        public string providerName { get; set; }

        [MaxLength(100, ErrorMessage = "Please enter between 1 and 100 characters.")]
        [Required(ErrorMessage = "Member ID is required")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Member ID must contain only alphanumeric characters.")]
        public string memberId { get; set; }

        [MaxLength(100, ErrorMessage = "Please enter between 1 and 100 characters.")]
        [Required(ErrorMessage = "Policy number is required")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Policy number must contain only alphanumeric characters.")]
        public string policyNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Please enter between 1 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Group number must contain alphanumeric characters.")]
        public string? groupNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Please enter between 1 and 100 characters.")]
        [Required(ErrorMessage = "Priority is required")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Priority must contain only alphabetic characters.")]
        public string priority { get; set; }

        [ForeignKey("primaryPhysician")]
        public Providers providers { get; set; }

        public int primaryPhysician {  get; set; }

        public bool active { get; set; }
    }
}
