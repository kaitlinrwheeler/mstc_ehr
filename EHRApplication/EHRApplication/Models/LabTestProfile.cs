using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class LabTestProfile
    {
        [Key]
        public int testId { get; set; }

        public string textName { get; set; }

        public string description { get; set; }

        public string units { get; set; }

        public string referenceRange { get; set; }

        public string category { get; set; }
    }
}
