using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class CarePlan
    {
        [Key]
        public int CPId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN {  get; set; }

        [ForeignKey("visitsId")]
        public Visits visits { get; set; }

        [Required(ErrorMessage = "Visit ID is required.")]
        [RegularExpression(@"^[0-9\s]*$", ErrorMessage = "Visit ID must contain only numbers.")]
        public int visitsId { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Priority must contain only alphanumeric characters.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Priority must be between 1 and 100 characters.")]
        public string priority { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime startDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime endDate { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Title must contain only alphanumeric characters.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters.")]
        public string title { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\p{P}]+$", ErrorMessage = "Title must contain only alphanumeric and punctuation characters.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Diagnosis must be between 1 and 100 characters.")]
        public string diagnosis { get; set; }
    }
}
