using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class LabResults
    {
        [Key]
        public int labId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("visitId")]
        public Visits visits { get; set; }

        public int visitsId { get; set; }

        [ForeignKey("testId")]
        public LabTestProfile labTests { get; set; }

        public int testId { get; set; }

        public string resultValue { get; set; }

        public string abnormalFlag { get; set; }

        [ForeignKey("orderedBy")]
        public Providers providers { get; set; }

        public int orderedBy { get; set; }

        public DateOnly date {  get; set; }

        public TimeOnly time { get; set; }
    }
}
