using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class PatientDemographic
    {
        [Key]
        public int MHN { get; set; }
    }
}
