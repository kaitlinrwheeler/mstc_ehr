using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace EHRApplication.Models
{
    public class PatientDx
    {
        [Key]
        public int patientDxId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public string Dx { get; set; }

        public DateTime createdAt { get; set; }

        [ForeignKey("createdBy")]
        public Providers providers { get; set; }

        public int createdBy { get; set; }

        public bool active { get; set; }
    }
}
