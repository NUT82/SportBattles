namespace SportBattles.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SportBattles.Services.JsonModels;

    public interface ILiveScoreApi
    {
        public IEnumerable<FootballMatchServiceModel> GetFootballMatches(DateTime startDate, DateTime endDate, string country = null, string tournament = null);

        public IEnumerable<TennisMatchServiceModel> GetTennisMatches(DateTime startDate, DateTime endDate, string country = null, string tournament = null);

        public IEnumerable<LeagueJson> GetCountriesAndTournaments(string jsonFileName);

        public Task CreateJsonFilesForAllMatchesAsync(DateTime startDate, DateTime endDate, string apiKey, string apiHost, string category);
    }
}
