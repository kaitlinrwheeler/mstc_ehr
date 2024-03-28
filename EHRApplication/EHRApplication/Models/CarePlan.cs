using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class CarePlan
    {
        [Key]
        public int CPId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN {  get; set; }

        [ForeignKey("visitsId")]
        public Visits visits { get; set; }

        public int visitsId { get; set; }

        public string priority { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        public string activeStatus { get; set; }

        public string title { get; set; }

        public string diagnosis { get; set; }
    }
}
