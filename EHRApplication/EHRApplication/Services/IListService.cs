using EHRApplication.Models;

namespace EHRApplication.Services
{
    public interface IListService
    {
        IEnumerable<Providers> GetProviders();

        IEnumerable<PatientContact> GetContacts();

        Providers GetProvidersByProviderId(int mhn);

        PatientContact GetContactsByMHN(int mhn);
    }
}
