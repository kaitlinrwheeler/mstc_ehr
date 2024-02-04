using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models.PatientDemographics
{
    public class AbdomenCoccyxGenitalia
    {
        [Key]
        public int ACGId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string abdominalShapeAppearance { get; set; }

        public string bowelSounds { get; set; }

        public string tendernessLumpDistention { get; set; }

        public string tubeDrainCath {  get; set; }

        public string skinAppearance { get; set; }

        [ForeignKey("providerId")]
        public Providers providers { get; set; }

        public int providerId { get; set; }
    }
}
