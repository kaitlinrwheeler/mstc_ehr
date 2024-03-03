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

        PatientContact GetContactByMHN(int mhn);

        MedicationProfile GetMedicationProfileByMedId(int medId);
    }
}
