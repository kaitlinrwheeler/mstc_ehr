using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class Allergies
    {
        [Key]
        public int allergyId { get; set; }

        public string allergyName { get; set; }

        public string allergyType { get; set;}
    }
}
