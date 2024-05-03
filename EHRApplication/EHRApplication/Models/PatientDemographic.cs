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

        [Required(ErrorMessage = "Please enter a first name.")]
        [StringLength(60, ErrorMessage = "First name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [StringLength(60, ErrorMessage = "Middle name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Middle Name")]
        public string? middleName { get; set; } // Use nullable reference types

        [Required(ErrorMessage = "Please enter a last name.")]
        [StringLength(60, ErrorMessage = "Last name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [StringLength(60, ErrorMessage = "Suffix must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Suffix")]
        public string? suffix { get; set; }

        [Required(ErrorMessage = "Please enter preferred pronouns.")]
        [StringLength(60, ErrorMessage = "Pronouns name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please select pronouns.")]
        [Display(Name = "Pronouns")]
        public string preferredPronouns { get; set; }

        [StringLength(60, ErrorMessage = "Pronouns name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please select pronouns.")]
        [Display(Name = "Pronouns")]
        [NotMapped]
        public string? OtherPronouns { get; set; }

        [Required(ErrorMessage = "Please enter a date of birth.")]
        [DataType(DataType.Date)]
        public DateOnly DOB { get; set; }

        [Required(ErrorMessage = "Please enter a gender.")]
        [StringLength(60, ErrorMessage = "Gender must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Gender")]
        public string gender { get; set; }

        [StringLength(60, ErrorMessage = "Gender must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Gender")]
        [NotMapped]
        public string? OtherGender { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        [StringLength(60, ErrorMessage = "Language must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Language")]
        public string preferredLanguage { get; set; }

        [Required(ErrorMessage = "Please select one.")]
        public string ethnicity { get; set; }

        [ValidateNever]
        public string race { get; set; }

        [Required(ErrorMessage = "Please select at least one race.")]
        [NotMapped]
        public List<string> raceList { get; set; }

        [StringLength(60, ErrorMessage = "Race must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Race")]
        [NotMapped]
        public string? OtherRace { get; set; }

        [StringLength(60, ErrorMessage = "Religion must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Religion")]
        public string? religion { get; set; }

        [ValidateNever]
        [ForeignKey("primaryPhysician")]
        public Providers providers { get; set; }

        [Required(ErrorMessage = "Primary physician is required.")]
        [Display(Name = "Primary physician")]
        public int primaryPhysician { get; set; }

        [StringLength(60, ErrorMessage = "Guardian1 must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Guardian1")]
        public string? legalGuardian1 { get; set; }

        [StringLength(60, ErrorMessage = "Guardian2 must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Guardian2")]
        public string? legalGuardian2 { get; set; }

        [StringLength(60, ErrorMessage = "Previous name must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "previousName")]
        public string? previousName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(60, ErrorMessage = "Gender must be between 1 and 60 characters.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s'\/\-]+$", ErrorMessage = "Please enter alphabetic characters only.")]
        [Display(Name = "Gender")]
        public string genderAssignedAtBirth { get; set; }

        public bool Active { get; set; }

        public bool HasAlerts { get; set; }

        // needs to be changed to accept a file
        // may need to add IFormFile as well 

        [NotMapped]
        [Display(Name = "Patient Image")]
        public IFormFile? patientImageFile { get; set; }

        [StringLength(60, ErrorMessage = "Image file must be of type .jpg, .jpeg, or .png.", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9]+\.(png|jpg|jpeg)$", ErrorMessage = "Please enter a valid file name.")]
        [Display(Name = "Patient Image")]
        public string? patientImage { get; set; }
    }
}
