using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class General
    {
        [Key]
        public int genId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string plainDiscomfort { get; set; }

        public string elimination { get; set; }

        public string appetite { get; set; }

        public string activity { get; set; }

        [ForeignKey("providerId")]
        public Providers providers { get; set; }

        public int providerId { get; set; }
    }
}
