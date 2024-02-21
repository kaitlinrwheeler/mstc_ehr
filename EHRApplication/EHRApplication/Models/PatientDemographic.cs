using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace EHRApplication.Models
{
    public class PatientDemographic
    {
        [Key]
        public int MHN { get; set; }
        [ValidateNever]
        public PatientContact ContactId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(60, ErrorMessage = "First name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the first name.")]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [StringLength(60, ErrorMessage = "Middle name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Only letters are allowed in the middle name.")]
        [Display(Name = "Middle Name")]
        public string? middleName { get; set; } // Use nullable reference types

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(60, ErrorMessage = "Last name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the last name.")]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [StringLength(60, ErrorMessage = "Suffix must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the Suffix.")]
        [Display(Name = "Suffix")]
        public string? suffix { get; set; }

        [StringLength(60, ErrorMessage = "Pronouns name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the pronoun.")]
        [Display(Name = "Pronouns")]
        public string? preferredPronouns { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(60, ErrorMessage = "Gender must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the Gender.")]
        [Display(Name = "Gender")]
        public string gender { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        [StringLength(60, ErrorMessage = "Language must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the Language.")]
        [Display(Name = "Language")]
        public string preferredLanguage { get; set; }

        [Required(ErrorMessage = "Ethnicity is required.")]
        [StringLength(60, ErrorMessage = "Ethnicity must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the ethnicity.")]
        [Display(Name = "Ethnicity")]
        public string ethnicity { get; set; }

        [Required(ErrorMessage = "At least one race must be selected")]
        public string race { get; set; }

        [StringLength(60, ErrorMessage = "Religion must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the Religion.")]
        [Display(Name = "Religion")]
        public string? religion { get; set; }

        [ValidateNever]
        [ForeignKey("primaryPhysician")]
        public Providers providers { get; set; }

        [Required(ErrorMessage = "Primary physician is required.")]
        [Display(Name = "Primary physician")]
        public int primaryPhysician { get; set; }
        
        [StringLength(60, ErrorMessage = "Guardian1 must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the Guardian1.")]
        [Display(Name = "Guardian1")]
        public string? legalGuardian1 { get; set; }

        [StringLength(60, ErrorMessage = "Guardian2 must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the Guardian2.")]
        [Display(Name = "Guardian2")]
        public string? legalGuardian2 { get; set; }

        [ValidateNever]
        public string previousName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(60, ErrorMessage = "Gender must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters are allowed in the Gender.")]
        [Display(Name = "Gender")]
        public string genderAssignedAtBirth { get; set; }
    }
}
