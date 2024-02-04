using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models.PatientDemographics
{
    public class HEENT_Neuro
    {
        [Key]
        public int HNId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string head { get; set; }

        public string vision { get; set; }

        public string hearing { get; set; }

        public string nose { get; set; }

        public string throatMouth { get; set; }

        [ForeignKey("providersId")]
        public Providers providers { get; set; }

        public int providerId { get; set; }
    }
}
