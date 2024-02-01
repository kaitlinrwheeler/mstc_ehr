using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class Visits
    {
        [Key]
        public int visitsId { get; set; }

        public PatientDemographic vatients { get; set; }

        public int MHN { get; set; }

        public Providers providers { get; set; }

        public int providerId { get; set; }

        public DateOnly date { get; set; }

        public TimeOnly time { get; set; }

        public bool admitted { get; set; }

        public string notes { get; set; }
    }
}
