using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class Providers
    {
        [Key]
        public int providerId { get; set; }

        [Required(ErrorMessage = "Please enter provider's first name.")]
        [RegularExpression(@"^[a-zA-Z\s\/\-'']*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Please enter provider's last name.")]
        [RegularExpression(@"^[a-zA-Z\s\/\-'']*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "Please enter provider's specialty.")]
        [RegularExpression(@"^[a-zA-Z\s\/\-'']*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string specialty { get; set; }

        public bool active { get; set; }
    }
}
