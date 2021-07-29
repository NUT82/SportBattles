namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class MatchesService : IMatchesService
    {
        private readonly IDeletableEntityRepository<Match> matchesRepository;
        private readonly IRepository<GameMatch> gameMatchRepository;

        public MatchesService(IDeletableEntityRepository<Match> matchesRepository, IRepository<GameMatch> gameMatchRepository)
        {
            this.matchesRepository = matchesRepository;
            this.gameMatchRepository = gameMatchRepository;
        }

        public async Task ChangeDoublePoints(int matchId, int gameId)
        {
            var gameMatch = this.gameMatchRepository.All().FirstOrDefault(gm => gm.GameId == gameId && gm.MatchId == matchId);

            gameMatch.DoublePoints = !gameMatch.DoublePoints;
            await this.gameMatchRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllByGameId<T>(int gameId)
        {
            return this.matchesRepository.AllAsNoTracking().Where(m => m.Games.Any(g => g.GameId == gameId)).OrderBy(m => m.StartTime).To<T>().ToList();
        }

        public IDictionary<int, bool> GetMatchesDoublePointsByGameId(int gameId)
        {
            return this.gameMatchRepository.AllAsNoTracking().Where(g => g.GameId == gameId).Select(m => new KeyValuePair<int, bool>(m.MatchId, m.DoublePoints)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
