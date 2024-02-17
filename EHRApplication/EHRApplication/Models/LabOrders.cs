using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class LabOrders
    {
        [Key]
        public int orderId {  get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("testId")]
        public LabTestProfile labTests { get; set; }

        public int testId { get; set; }

        [ForeignKey("visitsId")]
        public Visits visits { get; set; }

        public int visitsId { get; set; }

        public string completionStatus { get; set; }

        public DateOnly orderDate { get; set; }

        public TimeOnly orderTime { get; set; }

        [ForeignKey("orderedBy")]
        public Providers providers { get; set; }

        public int orderedBy { get; set; }
    }
}
