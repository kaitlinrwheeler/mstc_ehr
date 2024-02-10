using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class MedicationProfile
    {
        [Key]
        public int medId { get; set; }

        public string medName { get; set; }

        public string description { get; set; }

        public string route { get; set; }
    }
}
