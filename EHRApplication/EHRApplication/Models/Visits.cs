using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class Visits
    {
        [Key]
        public int visitsId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("providersId")]
        public Providers providers { get; set; }

        public int providerId { get; set; }

        public DateOnly date { get; set; }

        public TimeOnly time { get; set; }

        public bool admitted { get; set; }

        public string notes { get; set; }
    }
}
