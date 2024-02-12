using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models.PatientDemographics
{
    public class Skin
    {
        [Key]
        public int skinId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public DateOnly evaluationDate { get; set; }

        public TimeOnly evaluationTime { get; set; }

        public string woundsLessions { get; set; }

        public string rednessIrritation { get; set; }

        public string drynessIrritation { get; set; }

        public string colorTemp {  get; set; }

        public string signOfBreakdown { get; set; }

        [ForeignKey("providersId")]
        public Providers providers { get; set; }

        public int providerId { get; set; }
    }
}
