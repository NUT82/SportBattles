namespace SportBattles.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SportBattles.Services;

    public interface IMatchesService
    {
        public IEnumerable<T> GetAllByGameId<T>(int gameId);

        public DateTime GetStartTimeUTC(int matchId);

        public bool IsDoublePoint(int gameId, int matchId);

        public IDictionary<int, bool> GetMatchesDoublePointsByGameId(int gameId);

        public Task ChangeDoublePoints(int matchId, int gameId);

        public Task PopulateResults(IEnumerable<FootballMatchServiceModel> matches, DateTime startDate, DateTime endDate);
    }
}
