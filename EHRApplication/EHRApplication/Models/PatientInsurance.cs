﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRApplication.Models
{
    public class PatientInsurance
    {
        [Key]
        public int patientInsuranceId { get; set; }

        [ForeignKey("MHN")]
        public PatientDemographic patients {get; set; }

        public int MHN { get; set; }

        public string providerName { get; set; }

        public int memberId { get; set; }

        public int policyNumber { get; set; }

        public int groupNumber { get; set; }

        public string priority { get; set; }

        [ForeignKey("primaryPhysician")]
        public Providers providers { get; set; }

        public int primaryPhysician {  get; set; }

        public bool active { get; set; }
    }
}
