﻿using EHRApplication.Models;

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

        List<MedAdministrationHistory> GetMedAdministrationHistoryByMHN(int mhn);

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
        MedOrders GetMedOrderByOrderId(int orderId);

        PatientNotes GetPatientNotesByVisitId(int visistId);

        PatientProblems GetPatientProblemsByVisitId(int visitId);

        CarePlan GetCarePlanByVisitId(int visitId);

        MedAdministrationHistory GetMedHistoryByVisitId(int visitId);
        MedAdministrationHistory GetMedAdministrationHistoryByAdminId(int adminId);

        PatientProblems GetPatientProblemsByProblemId(int problemId);
        PatientMedications GetPatientsMedByPatientMedId(int patientMedId);
        public void InsertIntoProblems(PatientProblems problem);
        public void UpdateProblems(PatientProblems problem);
        public void InsertIntoPatientMed(PatientMedications medication);
        public void UpdatePatientMed(PatientMedications medication);

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

        void InsertIntoMedOrder(MedOrders medOrder);
        void UpdateMedOrder(MedOrders mrdOrder);

        void InsertIntoAdministrationHistory(MedAdministrationHistory medHistory);
        void UpdateAdministrationHistory(MedAdministrationHistory medHistory);
    }
}
