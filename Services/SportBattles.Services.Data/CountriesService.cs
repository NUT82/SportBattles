namespace SportBattles.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class CountriesService : ICountriesService
    {
        private readonly IDeletableEntityRepository<Country> countryRepository;
        private readonly IDeletableEntityRepository<Sport> sportRepository;

        public CountriesService(IDeletableEntityRepository<Country> countryRepository, IDeletableEntityRepository<Sport> sportRepository)
        {
            this.countryRepository = countryRepository;
            this.sportRepository = sportRepository;
        }

        public async Task<int> AddNewCountryToSport(int countryId, int sportId)
        {
            var country = this.countryRepository.All().Where(c => c.Id == countryId).FirstOrDefault();
            var sport = this.sportRepository.All().Where(s => s.Id == sportId).FirstOrDefault();
            if (country == null || sport == null)
            {
                throw new ArgumentNullException("Country or sport does not exist!");
            }

            if (country.Sports.Any(s => s.Id == sportId))
            {
                return country.Id;
            }

            country.Sports.Add(sport);
            await this.countryRepository.SaveChangesAsync();
            return country.Id;
        }

        public IEnumerable<T> GetAllOtherCountriesForSport<T>(int sportId)
        {
            return this.countryRepository.AllAsNoTracking().Where(c => !c.Sports.Any(s => s.Id == sportId)).OrderBy(c => c.Name).To<T>().ToList();
        }

        public IEnumerable<T> GetCountriesForSport<T>(int sportId)
        {
            return this.countryRepository.AllAsNoTracking().Where(c => c.Sports.Any(s => s.Id == sportId)).OrderBy(c => c.Name).To<T>().ToList();
        }
    }
}
