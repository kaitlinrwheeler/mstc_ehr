using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class Providers
    {
        [Key]
        public int providerId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string specialty { get; set; }
    }
}
