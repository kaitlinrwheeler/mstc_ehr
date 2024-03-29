using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class MedicationProfile
    {
        [Key]
        public int medId { get; set; }

        [Required(ErrorMessage ="Please enter a medication.")]
        public string medName { get; set; }

        [Required(ErrorMessage ="Please enter a description.")]
        public string description { get; set; }

        [Required(ErrorMessage ="Please enter a route")]
        public string route { get; set; }
        public bool activeStatus { get; set; }
    }
}
