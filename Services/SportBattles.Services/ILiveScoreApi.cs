namespace SportBattles.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SportBattles.Services.JsonModels;

    public interface ILiveScoreApi
    {
        public IEnumerable<FootballMatch> GetFootballMatches(DateTime startDate, DateTime endDate, string country = null, string tournament = null);

        public IEnumerable<FootballLeagueJson> GetFootballCountriesAndTournaments(string jsonFileName);

        public Task CreateJsonFilesForAllFootballMatchesAsync(DateTime startDate, DateTime endDate, string apiKey, string apiHost);
    }
}
