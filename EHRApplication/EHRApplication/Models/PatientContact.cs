using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientContact
    {
        [Key]
        public int patientContactId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        public string address { get; set; }

        public string city { get; set; }
        
        public string state { get; set; }

        public int zipcode { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string ECFirstName { get; set; }

        public string ECLastName { get; set; }

        public string ECRelationship { get; set; }

        public string ECPhone { get; set; }
    }
}
