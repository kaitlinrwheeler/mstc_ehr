using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class LabResults
    {
        [Key]
        public int labId { get; set; }

        [ForeignKey("MHN")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("visitsId")]
        [ValidateNever]
        public Visits visits { get; set; }

        public int visitsId { get; set; }

        [ForeignKey("testId")]
        [ValidateNever]
        public LabTestProfile labTests { get; set; }

        public int testId { get; set; }

        [Required(ErrorMessage = "Please enter a result value.")]
        [RegularExpression(@"^[+-]?(?:\d*\.?\d+|[a-zA-Z]+)$", ErrorMessage = "Only alphanumeric characters are allowed.")]
        public string resultValue { get; set; }

        public string abnormalFlag { get; set; }

        [ForeignKey("orderedBy")]
        [ValidateNever]
        public Providers providers { get; set; }

        public int orderedBy { get; set; }

        public DateOnly date {  get; set; }

        public TimeOnly time { get; set; }
    }
}
