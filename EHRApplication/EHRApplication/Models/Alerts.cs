using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class Alerts
    {
        [Key]
        public int alertId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public string alertName { get; set; }

        public string activeStatus { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }
}
