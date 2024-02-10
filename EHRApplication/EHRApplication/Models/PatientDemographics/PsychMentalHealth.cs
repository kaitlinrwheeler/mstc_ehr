using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models.PatientDemographics
{
    public class PsychMentalHealth
    {
        [Key]
        public int psychId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string moodAffect {  get; set; }

        public string cognition { get; set; }

        public string thoughtPattern { get; set; }

        public string sleepPattern { get; set; }

        [ForeignKey("providerId")]
        public Providers providers { get; set; }

        public int providersId { get; set; }
    }
}
