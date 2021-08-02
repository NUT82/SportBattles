namespace SportBattles.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SportBattles.Common;
    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services;
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
            return this.matchesRepository.AllAsNoTracking().Where(m => m.Games.Any(g => g.GameId == gameId)).OrderByDescending(m => m.StartTime).To<T>().ToList();
        }

        public IDictionary<int, bool> GetMatchesDoublePointsByGameId(int gameId)
        {
            return this.gameMatchRepository.AllAsNoTracking().Where(g => g.GameId == gameId).Select(m => new KeyValuePair<int, bool>(m.MatchId, m.DoublePoints)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public DateTime GetStartTimeUTC(int matchId)
        {
            return this.matchesRepository.AllAsNoTracking().FirstOrDefault(m => m.Id == matchId).StartTime;
        }

        public async Task PopulateYesterdayResult(IEnumerable<FootballMatch> matches)
        {
            matches = matches.OrderBy(m => m.StartTimeUTC);
            var startDate = DateTime.UtcNow.AddDays(-1).Date.AddHours(-GlobalConstants.LiveScoreAPITimeZoneCorrection);
            var endDate = DateTime.UtcNow.Date.AddHours(-GlobalConstants.LiveScoreAPITimeZoneCorrection);
            var yesterdayMatches = this.matchesRepository.All()
                .Where(m => m.StartTime >= startDate && m.StartTime <= endDate)
                .ToList();

            foreach (var match in yesterdayMatches.Where(m => m.HomeGoals == null || m.AwayGoals == null))
            {
                var currentMatch = matches.Where(m => m.HomeTeam == match.HomeTeam.Name && m.AwayTeam == match.AwayTeam.Name).FirstOrDefault();
                if (currentMatch == null || (currentMatch.Status != "FT" && currentMatch.Status != "AP" && currentMatch.Status != "AET"))
                {
                    continue;
                }

                match.HomeGoals = currentMatch.HomeGoals;
                match.AwayGoals = currentMatch.AwayGoals;
                match.HomeGoalsFirstHalf = currentMatch.HalfHomeGoals;
                match.AwayGoalsFirstHalf = currentMatch.HalfAwayGoals;
            }

            await this.matchesRepository.SaveChangesAsync();
        }
    }
}
