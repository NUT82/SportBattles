namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountriesService
    {
        IEnumerable<T> GetCountriesForSport<T>(int sportId);

        Task AddNewCountryToSport(string name, int sportId);
    }
}
