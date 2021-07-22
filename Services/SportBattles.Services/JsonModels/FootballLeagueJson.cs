namespace SportBattles.Services.JsonModels
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class FootballLeagueJson
    {
        [JsonProperty("Cnm")]
        public string Country { get; set; }

        [JsonProperty("Stages")]
        public List<Tournament> Tournaments { get; set; }

        public class Tournament
        {
            [JsonProperty("Sdn")]
            public string Name { get; set; }
        }
    }
}
