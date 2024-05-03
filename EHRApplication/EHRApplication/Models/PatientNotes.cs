using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientNotes
    {
        [Key]
        public int patientNotesId { get; set; }

        public PatientDemographic patients { get; set; }

        [ForeignKey("MHN")]
        public int MHN { get; set; }

        [Required(ErrorMessage = "Note is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Note must be between 1 and 100 characters.")]
        public string Note {  get; set; }

        [ForeignKey("visitsId")]
        public Visits visits { get; set; }

        public int visitsId { get; set; }

        [Required(ErrorMessage = "Date occured on is required.")]
        public DateOnly occurredOn { get; set; }

        public DateTime createdAt { get; set; }

        public Providers providers { get; set; }

        [ForeignKey("createdBy")]
        [Required(ErrorMessage = "Person who created the note is required.")]
        public int createdBy { get; set; }

        [ForeignKey("associatedProvider")]
        [Required(ErrorMessage = "Assosciated provider is required.")]
        public int associatedProvider {  get; set; }

        public Providers assocProvider {  get; set; }

        public DateTime updatedAt { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Note must be between 1 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Category must contain only letters only.")]
        public string category { get; set; }
    }
}
