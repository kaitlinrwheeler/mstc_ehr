using EHRApplication.Models;

namespace EHRApplication.ViewModels
{
    public class PortalViewModel
    {
        public PatientDemographic PatientDemographic { get; set; }
        public List<Alerts> Alerts { get; set; }
        public List<Allergies> Allergies { get; set; }
        public List<CarePlan> CarePlans { get; set; }
        public List<LabOrders> LabOrders { get; set; }
        public List<LabResults> LabResults { get; set; }
        public List<LabTestProfile> LabTests { get; set; }
        public List<MedAdministrationHistory> MedAdministrationHistories { get; set; }
        public List<MedicationProfile> Medications { get; set; }
        public MedicationProfile MedicationProfile { get; set; }
        public List<MedOrders> MedOrders { get; set; }
        public List<PatientAllergies> PatientAllergies { get; set; }
        public List<PatientContact> PatientContacts { get; set; }
        public PatientContact PatientContact { get; set; }
        public List<PatientInsurance> PatientInsurance { get; set; }
        public List<PatientMedications> PatientMedications { get; set; }
        public List<PatientNotes> PatientNotes { get; set; }
        public List<PatientProblems> PatientProblems { get; set; }
        public List<Providers> Providers { get; set; }
        public List<Visits> Visits { get; set; }
        public List<Vitals> Vitals { get; set; }
        public Visits VisitDetails { get; set; }
        public CarePlan CarePlansDetails { get; set; }
        public LabOrders LabOrdersDetails { get; set; }
        public LabResults LabResultsDetails { get; set; }
        public MedAdministrationHistory MedAdministrationHistoriesDetails { get; set; }
        public MedOrders MedOrdersDetails { get; set; }
        public PatientNotes PatientNotesDetails { get; set; }
        public PatientProblems PatientProblemsDetails { get; set; }
        public Visits VisitDetailsDetails { get; set; }
        public Vitals VitalsDetails { get; set; }
    }
}
