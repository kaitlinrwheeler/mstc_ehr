using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models.PatientDemographics
{
    public class Respiratory
    {
        [Key]
        public int respId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string lungSounds { get; set; }

        public string respirationDepth { get; set; }

        public string respirationRate { get; set; }

        public string chestShapeAppearance { get; set; }

        public string drainLineSutureStaple { get; set; }

        [ForeignKey("providerId")]
        public Providers providers { get; set; }

        public int providerId { get; set; }
    }
}
