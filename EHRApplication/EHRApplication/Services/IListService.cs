using EHRApplication.Models;

namespace EHRApplication.Services
{
    public interface IListService
    {
        IEnumerable<PatientContact> GetContacts();
        IEnumerable<Providers> GetProviders();

        IEnumerable<PatientDemographic> GetPatients();

        Allergies GetAllergyByAllergyId(int allergyId);

        IEnumerable<MedicationProfile> GetMedicationProfiles();

        Providers GetProvidersByProviderId(int mhn);

        LabTestProfile GetLabTestByTestId(int testId);
        List<LabTestProfile> GetLabTests();

        PatientContact GetContactByMHN(int mhn);

        MedicationProfile GetMedicationProfileByMedId(int medId);

        List<MedAdministrationHistory> GetPatientsMedHistoryByMHN(int mhn);

        List<LabResults> GetPatientsLabResultsByMHN(int mhn);

        List<Visits> GetPatientVisitsByMHN(int mhn);

        List<CarePlan> GetCarePlanByMHN(int mhn);

        List<LabOrders>GetPatientsLabOrdersByMHN(int mhn);

        PatientDemographic GetPatientByMHN(int mhn);

        Visits GetVisitByVisitId(int visitId);

        Vitals GetVitalsByVisitId(int visitId);
        Vitals GetVitalsByVitalsId(int visitId);

        LabResults GetLabResultsByVisitId(int visitId);

        LabOrders GetLabOrdersByVisitId(int visitId);

        MedOrders GetMedOrdersByVisitId(int visitId);

        PatientNotes GetPatientNotesByVisitId(int visistId);

        PatientProblems GetPatientProblemsByVisitId(int visitId);

        CarePlan GetCarePlanByVisitId(int visitId);

        MedAdministrationHistory GetMedHistoryByVisitId(int visitId);

        LabOrders GetLabOrderByOrderId(int orderId);
        LabResults GetLabResultByLabId(int labId);

        void InsertIntoVisits(Visits visit);
        void UpdateVisits(Visits visit);

        void UpdateLabOrders(LabOrders labOrders);
        void InsertIntoLabOrders(LabOrders labOrders);

        void InsertIntoLabResults(LabResults labResults);
        void UpdateLabResults(LabResults labResults);

        void InsertIntoVitals(Vitals vital);
        void UpdateVitals(Vitals vital);

        public decimal BMICalculator(decimal weight, decimal height);

        void InsertIntoMedProfile(MedicationProfile medProfile);
        void UpdateMedProfile(MedicationProfile medProfile);
        Providers GetProviderById(int providerId);
        void UpdateProvider(Providers provider);
        void AddProvider(Providers provider);
    }
}
