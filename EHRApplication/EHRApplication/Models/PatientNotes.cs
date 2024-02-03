using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientNotes
    {
        [Key]
        public int patientNotesId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public string Note {  get; set; }

        public DateOnly occurredOn { get; set; }

        public DateTime createdAt { get; set; }

        [ForeignKey("createdBy")]
        public Providers providers { get; set; }

        public int createdBy { get; set; }

        public int associatedProvider {  get; set; }

        public DateTime updatedAt { get; set; }

        public string category { get; set; }
    }
}
