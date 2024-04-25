using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class LabOrders
    {
        [Key]
        public int orderId {  get; set; }

        [ForeignKey("MHN")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("testId")]
        [ValidateNever]
        public LabTestProfile labTests { get; set; }

        public int testId { get; set; }

        [ForeignKey("visitsId")]
        [ValidateNever]
        public Visits visits { get; set; }

        public int visitsId { get; set; }

        [Required(ErrorMessage = "Please select the status of the order.")]
        public string completionStatus { get; set; }

        public DateOnly orderDate { get; set; }

        public TimeOnly orderTime { get; set; }

        [ForeignKey("orderedBy")]
        [ValidateNever]
        public Providers providers { get; set; }

        public int orderedBy { get; set; }
    }
}
