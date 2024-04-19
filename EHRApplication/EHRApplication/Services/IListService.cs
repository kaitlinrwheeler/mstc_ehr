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

        PatientProblems GetPatientProblemsByProblemId(int problemId);
        PatientMedications GetPatientsMedByPatientMedId(int patientMedId);
        public void InsertIntoProblems(PatientProblems problem);
        public void UpdateProblems(PatientProblems problem);
        public void InsertIntoPatientMed(PatientMedications medication);
        public void UpdatePatientMed(PatientMedications medication);

        void InsertIntoVisits(Visits visit);

        void UpdateVisits(Visits visit);

        void InsertIntoVitals(Vitals vital);

        void UpdateVitals(Vitals vital);

        public decimal BMICalculator(decimal weight, decimal height);

        void InsertIntoMedProfile(MedicationProfile medProfile);
        void UpdateMedProfile(MedicationProfile medProfile);
    }
}
