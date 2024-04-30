using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class Alerts
    {
        [Key]
        public int alertId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [Required(ErrorMessage = "Please enter alert name.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Alert name must be between 5 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string alertName { get; set; }

        public bool activeStatus { get; set; }

        [Required(ErrorMessage = "Please enter a start date.")]
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }

        [Required(ErrorMessage = "Please enter an end date.")]
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }
    }
}
