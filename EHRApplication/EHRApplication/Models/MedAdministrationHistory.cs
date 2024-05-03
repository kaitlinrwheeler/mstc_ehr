using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class MedAdministrationHistory
    {
        [Key]
        public int administrationId { get; set; }

        [ForeignKey("MHN")]
        [ValidateNever]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [ForeignKey("visitsId")]
        [ValidateNever]
        public Visits visits { get; set; }

        public int visitsId { get; set; }

        public string category { get; set; }

        [ForeignKey("medId")]
        [ValidateNever]
        public MedicationProfile medProfile { get; set; }

        public int medId { get; set; }

        public string status { get; set; }

        public string frequency { get; set; }

        public DateOnly dateGiven { get; set; }

        public TimeOnly timeGiven { get; set; }

        [ForeignKey("administeredBy")]
        [ValidateNever]
        public Providers providers { get; set; }

        public int administeredBy { get; set; }

        public static implicit operator List<object>(MedAdministrationHistory v)
        {
            throw new NotImplementedException();
        }
    }
}
