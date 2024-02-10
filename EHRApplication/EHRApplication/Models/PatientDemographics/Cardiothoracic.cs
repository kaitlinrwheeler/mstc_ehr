using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models.PatientDemographics
{
    public class Cardiothoracic
    {
        [Key]
        public int cardioId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string heartSounds { get; set; }

        public string heartRhythm { get; set; }

        public string heartRate { get; set; }

        public string jugularVenousPulse { get; set; }

        public string drainLineSutureStaple { get; set; }

        [ForeignKey("providerId")]
        public Providers providers { get; set; }

        public int providerId { get; set; }
    }
}
