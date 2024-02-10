using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models.PatientDemographics
{
    public class Extremities
    {
        [Key]
        public int exId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string colorTempCapillaryRefill { get; set; }

        public string pulseSensation {  get; set; }

        public string edema {  get; set; }

        public string rangeOfMotion { get; set; }

        public string tubeDrainSutureStapleCath { get; set; }

        [ForeignKey("providerId")]
        public Providers providers { get; set; }

        public int providerId { get; set; }
    }
}
