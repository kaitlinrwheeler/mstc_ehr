using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class Allergies
    {
        [Key]
        public int allergyId { get; set; }

        [Required(ErrorMessage = "Please enter allergy name.")]
        [StringLength(100, ErrorMessage = "Allergy name must not exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s\/\-'']*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string allergyName { get; set; }

        [Required(ErrorMessage = "Please enter the allergy type.")]
        [StringLength(100, ErrorMessage = "Allergy type must not exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s\/\-'']*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string allergyType { get; set;}

        public bool activeStatus { get; set; }
    }
}
