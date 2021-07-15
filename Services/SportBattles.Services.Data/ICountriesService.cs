namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountriesService
    {
        IEnumerable<T> GetCountriesForSport<T>(int sportId);

        IEnumerable<T> GetAllOtherCountriesForSport<T>(int sportId);

        Task<int> AddNewCountryToSport(int countryId, int sportId);
    }
}
