using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientProblems
    {
        [Key]
        public int patientProblemsId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public string priority { get; set; }

        public string descripcion { get; set; }

        public string ICD_10 { get; set; }

        public string immediacy { get; set; }

        public DateTime createdAt { get; set; }

        [ForeignKey("createdBy")]
        public Providers providers { get; set; }

        public int createdBy { get; set; }

        public bool actived { get; set; }
    }
}
