namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class TournamentsService : ITournamentsService
    {
        private readonly IDeletableEntityRepository<Tournament> tournamentRepository;

        public TournamentsService(IDeletableEntityRepository<Tournament> tournamentRepository)
        {
            this.tournamentRepository = tournamentRepository;
        }

        public IEnumerable<T> GetAllForSportInCountry<T>(int sportId, int countryId)
        {
            return this.tournamentRepository.AllAsNoTracking().Where(t => t.SportId == sportId && t.CountryId == countryId).OrderBy(t => t.Name).To<T>().ToList();
        }
    }
}
