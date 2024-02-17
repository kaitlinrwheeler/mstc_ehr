using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class MedOrders
    {
        [Key]
        public int orderId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("visitId")]
        public Visits visits { get; set; }

        public int visitId { get; set; }

        [ForeignKey("medId")]
        public MedicationProfile medProfile { get; set; }

        public int medId { get; set; }

        public string frequency { get; set; }

        public string fulfillmentStatus { get; set; }

        public DateOnly orderDate { get; set; }

        public TimeOnly orderTime { get; set; }

        [ForeignKey("orderedBy")]
        public Providers providers { get; set; }

        public int orderedBy { get; set; }
    }
}
