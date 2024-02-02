namespace EHRApplication.Models
{
    public class PatientModel
    {
        public int MHN { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string PreferredLanguage { get; set; }
        public string Ethnicity { get; set; }
        public string Race { get; set; }
        public string Religion { get; set; }
        public int PrimaryPhysician { get; set; }
        public string LegalGuardian1 { get; set; }
        public string LegalGuardian2 { get; set; }
    }

}
