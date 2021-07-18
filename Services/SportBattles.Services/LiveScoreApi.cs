namespace SportBattles.Services
{
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    using SportBattles.Services.JsonModels;

    public class LiveScoreApi : ILiveScoreApi
    {
        public IEnumerable<FootballMatch> GetFootballMatches(string jsonFileName, string country = null, string tournament = null)
        {
            var jsonObj = JObject.Parse(System.IO.File.ReadAllText(jsonFileName));
            var matches = jsonObj["Stages"].ToObject<IEnumerable<FootballMatch>>();

            if (country == null)
            {
                return matches;
            }

            return null;
        }
    }
}
