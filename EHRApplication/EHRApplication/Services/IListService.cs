using EHRApplication.Models;

namespace EHRApplication.Services
{
    public interface IListService
    {
        IEnumerable<PatientContact> GetContacts();
        IEnumerable<Providers> GetProviders();

        IEnumerable<MedicationProfile> GetMedicationProfiles();

        Providers GetProvidersByProviderId(int mhn);

        PatientContact GetContactsByMHN(int mhn);

    }
}
