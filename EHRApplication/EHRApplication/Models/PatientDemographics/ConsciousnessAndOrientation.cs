using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models.PatientDemographics
{
    public class ConsciousnessAndOrientation
    {
        [Key]
        public int COId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string person { get; set; }

        public string place { get; set; }

        public string time { get; set; }

        public string purpose { get; set; }

        [ForeignKey("providerId")]
        public Providers providers { get; set; }

        public int providerId { get; set; }
    }
}
