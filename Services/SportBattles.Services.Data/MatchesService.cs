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
            var notStarted = this.matchesRepository.AllAsNoTracking().Where(m => m.Games.Any(g => g.GameId == gameId) && m.StartTime > DateTime.UtcNow).OrderBy(m => m.StartTime).To<T>().ToList();
            var startedOrFinished = this.matchesRepository.AllAsNoTracking().Where(m => m.Games.Any(g => g.GameId == gameId) && m.StartTime <= DateTime.UtcNow).OrderByDescending(m => m.StartTime).To<T>().ToList();

            return notStarted.Concat(startedOrFinished);
        }

        public IDictionary<int, bool> GetMatchesDoublePointsByGameId(int gameId)
        {
            return this.gameMatchRepository.AllAsNoTracking().Where(g => g.GameId == gameId).Select(m => new KeyValuePair<int, bool>(m.MatchId, m.DoublePoints)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public DateTime GetStartTimeUTC(int matchId)
        {
            return this.matchesRepository.AllAsNoTracking().FirstOrDefault(m => m.Id == matchId).StartTime;
        }

        public bool IsDoublePoint(int gameId, int matchId)
        {
            var gameMatch = this.gameMatchRepository.AllAsNoTracking().FirstOrDefault(gm => gm.GameId == gameId && gm.MatchId == matchId);
            if (gameMatch == null)
            {
                throw new NullReferenceException("No GameMatch with this Id's");
            }

            return gameMatch.DoublePoints;
        }

        public async Task PopulateResults(IEnumerable<FootballMatchServiceModel> matches, DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date.AddHours(-GlobalConstants.LiveScoreAPITimeZoneCorrection);
            endDate = endDate.Date.AddHours(-GlobalConstants.LiveScoreAPITimeZoneCorrection);
            var yesterdayMatches = this.matchesRepository.All()
                .Where(m => m.StartTime >= startDate && m.StartTime <= endDate)
                .ToList();

            foreach (var match in yesterdayMatches.Where(m => m.HomeGoals == null || m.AwayGoals == null))
            {
                var currentMatch = matches.Where(m => m.HomeTeam == match.HomeTeam.Name && m.AwayTeam == match.AwayTeam.Name).FirstOrDefault();
                if (currentMatch == null || currentMatch.Status == "NS" || currentMatch.Status == "HT" || currentMatch.Status.Contains("'"))
                {
                    continue;
                }

                match.HomeGoals = currentMatch.HomeGoals;
                match.AwayGoals = currentMatch.AwayGoals;
                match.HomeGoalsFirstHalf = currentMatch.HalfHomeGoals;
                match.AwayGoalsFirstHalf = currentMatch.HalfAwayGoals;
                match.Status = currentMatch.Status;
            }

            await this.matchesRepository.SaveChangesAsync();
        }
    }
}
