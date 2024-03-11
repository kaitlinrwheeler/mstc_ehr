using EHRApplication.Models;

namespace EHRApplication.Services
{
    public interface IListService
    {
        IEnumerable<PatientContact> GetContacts();
        IEnumerable<Providers> GetProviders();

        IEnumerable<PatientDemographic> GetPatients();

        IEnumerable<MedicationProfile> GetMedicationProfiles();

        Providers GetProvidersByProviderId(int mhn);

        LabTestProfile GetLabTestByTestId(int testId);

        PatientContact GetContactByMHN(int mhn);

        MedicationProfile GetMedicationProfileByMedId(int medId);

        List<MedAdministrationHistory> GetPatientsMedHistoryByMHN(int mhn);

        List<LabResults> GetPatientsLabResultsByMHN(int mhn);

        List<Visits> GetPatientVisitsByMHN(int mhn);

        List<CarePlan> GetCarePlanByMHN(int mhn);
    }
}
