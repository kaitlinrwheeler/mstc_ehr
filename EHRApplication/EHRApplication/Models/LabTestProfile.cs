using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class LabTestProfile
    {
        [Key]
        public int testId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
        public string testName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description must not exceed 100 characters")]
        public string description { get; set; }

        [Required(ErrorMessage = "Units is required")]
        [StringLength(100, ErrorMessage = "Units must not exceed 100 characters")]
        public string units { get; set; }

        [Required(ErrorMessage = "Reference range is required")]
        [StringLength(100, ErrorMessage = "Reference range  must not exceed 100 characters")]
        public string referenceRange { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(100, ErrorMessage = "Category must not exceed 100 characters")]
        public string category { get; set; }

        public bool Active { get; set; }
    }
}
