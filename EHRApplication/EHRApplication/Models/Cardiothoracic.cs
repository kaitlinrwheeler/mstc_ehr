using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class Cardiothoracic
    {
        [Key]
        public int cardioId { get; set; }

        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string heartSounds { get; set; }

        public string heartRhythm { get; set; }

        public string heartRate { get; set; }

        public string jugularVenousPulse { get; set; }
    }
}
