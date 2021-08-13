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

    public class TennisMatchesService : ITennisMatchesService
    {
        private readonly IDeletableEntityRepository<TennisMatch> tennisMatchesRepository;
        private readonly IRepository<GameTennisMatch> gameTennisMatchRepository;

        public TennisMatchesService(IDeletableEntityRepository<TennisMatch> tennisMatchesRepository, IRepository<GameTennisMatch> gameTennisMatchRepository)
        {
            this.tennisMatchesRepository = tennisMatchesRepository;
            this.gameTennisMatchRepository = gameTennisMatchRepository;
        }

        public async Task ChangeDoublePoints(int matchId, int gameId)
        {
            var gameMatch = this.gameTennisMatchRepository.All().FirstOrDefault(gm => gm.GameId == gameId && gm.TennisMatchId == matchId);

            gameMatch.DoublePoints = !gameMatch.DoublePoints;
            await this.gameTennisMatchRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllByGameId<T>(int gameId)
        {
            var notStarted = this.tennisMatchesRepository.AllAsNoTracking().Where(m => m.Games.Any(g => g.GameId == gameId) && m.StartTime > DateTime.UtcNow).OrderBy(m => m.StartTime).To<T>().ToList();
            var startedOrFinished = this.tennisMatchesRepository.AllAsNoTracking().Where(m => m.Games.Any(g => g.GameId == gameId) && m.StartTime <= DateTime.UtcNow).OrderByDescending(m => m.StartTime).To<T>().ToList();

            return notStarted.Concat(startedOrFinished);
        }

        public IEnumerable<T> GetAllWithoutResult<T>()
        {
            return this.tennisMatchesRepository.AllAsNoTracking().Where(m => m.HomeSets == null && m.StartTime.Date >= DateTime.UtcNow.Date && m.StartTime.Date < DateTime.UtcNow.Date.AddDays(GlobalConstants.LiveScoreAPIDaysAheadForTennis)).OrderBy(m => m.Tournament.Country.Name).ThenBy(m => m.Tournament.Name).ThenBy(m => m.StartTime).To<T>().ToList();
        }

        public IEnumerable<T> GetAllWithResult<T>()
        {
            return this.tennisMatchesRepository.AllAsNoTracking().Where(m => m.HomeSets != null && m.StartTime.Date > DateTime.UtcNow.Date.AddDays(-GlobalConstants.LiveScoreAPIDaysAheadForFootball) && m.StartTime.Date <= DateTime.UtcNow.Date).OrderBy(m => m.Tournament.Country.Name).ThenBy(m => m.Tournament.Name).ThenBy(m => m.StartTime).To<T>().ToList();
        }

        public IDictionary<int, bool> GetMatchesDoublePointsByGameId(int gameId)
        {
            return this.gameTennisMatchRepository.AllAsNoTracking().Where(g => g.GameId == gameId).Select(m => new KeyValuePair<int, bool>(m.TennisMatchId, m.DoublePoints)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public DateTime GetStartTimeUTC(int matchId)
        {
            return this.tennisMatchesRepository.AllAsNoTracking().FirstOrDefault(m => m.Id == matchId).StartTime;
        }

        public bool IsDoublePoint(int gameId, int matchId)
        {
            var gameMatch = this.gameTennisMatchRepository.AllAsNoTracking().FirstOrDefault(gm => gm.GameId == gameId && gm.TennisMatchId == matchId);
            if (gameMatch == null)
            {
                throw new NullReferenceException("No GameMatch with this Id's");
            }

            return gameMatch.DoublePoints;
        }

        public async Task PopulateResults(IEnumerable<TennisMatchServiceModel> matches, DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date.AddHours(-GlobalConstants.LiveScoreAPITimeZoneCorrection);
            endDate = endDate.Date.AddHours(-GlobalConstants.LiveScoreAPITimeZoneCorrection);
            var yesterdayMatches = this.tennisMatchesRepository.All()
                .Where(m => m.StartTime >= startDate && m.StartTime <= endDate)
                .ToList();

            foreach (var match in yesterdayMatches.Where(m => m.HomeSets == null || m.AwaySets == null))
            {
                var currentMatch = matches.Where(m => m.HomeTeam == match.HomePlayer.Name && m.AwayTeam == match.AwayPlayer.Name).FirstOrDefault();
                if (currentMatch == null || currentMatch.Status == "NS" || currentMatch.Status == "HT" || currentMatch.Status.Contains("'"))
                {
                    continue;
                }

                match.HomeSets = currentMatch.HomeSets;
                match.AwaySets = currentMatch.AwaySets;
                ////TODO: get games
                match.Status = currentMatch.Status;
            }

            await this.tennisMatchesRepository.SaveChangesAsync();
        }
    }
}
