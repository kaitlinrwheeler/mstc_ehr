using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class MedOrders
    {
        [Key]
        public int orderId { get; set; }

        [ForeignKey("MHN")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("visitId")]
        [ValidateNever]
        public Visits visits { get; set; }

        public int visitId { get; set; }

        [ForeignKey("medId")]
        [ValidateNever]
        public MedicationProfile medProfile { get; set; }

        public int medId { get; set; }

        [Required(ErrorMessage = "Please enter a frequency.")]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string frequency { get; set; }

        public string fulfillmentStatus { get; set; }

        public DateOnly orderDate { get; set; }

        public TimeOnly orderTime { get; set; }

        [ForeignKey("orderedBy")]
        [ValidateNever]
        public Providers providers { get; set; }

        public int orderedBy { get; set; }
    }
}
