namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class CountriesService : ICountriesService
    {
        private readonly IDeletableEntityRepository<Country> countryRepository;

        public CountriesService(IDeletableEntityRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public Task AddNewCountryToSport(string name, int sportId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetCountriesForSport<T>(int sportId)
        {
            return this.countryRepository.AllAsNoTracking().Where(c => c.Sports.Any(s => s.Id == sportId)).OrderBy(c => c.Name).To<T>().ToList();
        }
    }
}
