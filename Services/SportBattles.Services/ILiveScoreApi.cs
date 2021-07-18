namespace SportBattles.Services
{
    using System.Collections.Generic;

    using SportBattles.Services.JsonModels;

    public interface ILiveScoreApi
    {
        public IEnumerable<FootballMatch> GetFootballMatches(string jsonFileName, string country = null, string tournament = null);
    }
}
