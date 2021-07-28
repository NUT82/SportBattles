namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class MatchesService : IMatchesService
    {
        private readonly IDeletableEntityRepository<Match> matchesRepository;

        public MatchesService(IDeletableEntityRepository<Match> matchesRepository)
        {
            this.matchesRepository = matchesRepository;
        }

        public IEnumerable<T> GetAllByGameId<T>(int gameId)
        {
            return this.matchesRepository.AllAsNoTracking().Where(m => m.Games.Any(g => g.Id == gameId)).OrderBy(m => m.StartTime).To<T>().ToList();
        }
    }
}
