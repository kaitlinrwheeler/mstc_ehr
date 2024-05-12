﻿using System.ComponentModel.DataAnnotations;

namespace EHRApplication.Models
{
    public class MedicationProfile
    {
        [Key]
        public int medId { get; set; }

        [Required(ErrorMessage ="Please enter a medication.")]
        public string medName { get; set; }

        [Required(ErrorMessage ="Please enter a description.")]
        [StringLength(100, ErrorMessage = "Description must nut exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s.,'""!?()\-]*$", ErrorMessage = "Description must contain only alphanumeric characters and standard punctuation.")]
        public string description { get; set; }

        [Required(ErrorMessage ="Please enter a route")]
        public string route { get; set; }
        public bool activeStatus { get; set; }
    }
}
