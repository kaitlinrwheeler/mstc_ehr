using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class Log
    {
        [Key]
        public int LogID { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public string Context { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
