using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class Medications
    {
        [Key]
        public int medId { get; set; }

        public string medName { get; set; }

        public string description { get; set; }

        public string sideEffects { get; set; }

        public string manufacturer { get; set; }

        public string route { get; set; }

        public string storageRequirements { get; set; }

        public string contraindications { get; set; }

        public DateOnly date { get; set; }

        public string category { get; set; }

        public bool homeMeds { get; set; }

        public string author { get; set; }

        public string provider { get; set; }

        public string medication { get; set; }

        public int includeDEA_NPINumber {get; set;}

        public string alternateName { get; set; }   
        
        public int barcodeId { get; set; }

        public string orderDetails { get; set; }

        public string frequency { get; set; }

        public string status { get; set; }

        public DateOnly startsOn { get; set; }

        public DateOnly endsOn { get; set; }
    }
}
