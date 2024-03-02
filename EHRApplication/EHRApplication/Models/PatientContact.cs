using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientContact
    {
        [Key]
        public int patientContactId { get; set; }

        [ValidateNever]
        [ForeignKey("MHN")]
        public PatientDemographic patients { get; set; }

        public int MHN { get; set; }

        [Required(ErrorMessage = "Please enter an address.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\/\-'']*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string address { get; set; }

        [Required(ErrorMessage = "Please enter a city.")]
        [RegularExpression(@"^[a-zA-Z\s\/\-'']*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string city { get; set; }

        [Required(ErrorMessage = "Please enter a state.")]
        public string state { get; set; }

        [Required(ErrorMessage = "Please enter a zipcode.")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zipcode")]
        public int? zipcode { get; set; }

        [Required(ErrorMessage = "Please enter a phone number.")]
        [RegularExpression(@"^[0-9\s\+\-]*$", ErrorMessage = "Please enter numbers only.")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Please enter a email.")]
        [RegularExpression(@"^[^@\s]+@([a-zA-Z0-9]+\.)+(com|net|edu|gov)$", ErrorMessage = "Please enter a valid email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string email { get; set; }

        [RegularExpression(@"^[a-zA-Z\s\/\-]*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string? ECFirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z\s\/\-]*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string? ECLastName { get; set; }

        [RegularExpression(@"^[a-zA-Z\s\/\-]*$", ErrorMessage = "Please enter alphabetic characters only.")]
        public string? ECRelationship { get; set; }

        [RegularExpression(@"^[0-9\s\+\-]*$", ErrorMessage = "Please enter numbers only.")]
        public string? ECPhone { get; set; }
    }
}
