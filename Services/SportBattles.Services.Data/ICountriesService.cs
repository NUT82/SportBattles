namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountriesService
    {
        IEnumerable<T> GetAllForSport<T>(int sportId);

        IEnumerable<T> GetAllOthersForSport<T>(int sportId);

        Task<int> AddToSport(int countryId, int sportId);
    }
}
