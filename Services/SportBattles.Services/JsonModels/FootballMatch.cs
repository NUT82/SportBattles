namespace SportBattles.Services.JsonModels
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class FootballMatch
    {
        [JsonProperty("Snm")]
        public string Tournament { get; set; }

        [JsonProperty("Cnm")]
        public string Country { get; set; }

        public IList<Event> Events { get; set; }

        public class Event
        {
            [JsonProperty("Eps")]
            public string Status { get; set; }

            [JsonProperty("Tr1")]
            public string HomeGoals { get; set; }

            [JsonProperty("Tr2")]
            public string AwayGoals { get; set; }

            [JsonProperty("Trh1")]
            public string HomeGoalsFirstHalf { get; set; }

            [JsonProperty("Trh2")]
            public string AwayGoalsFirstHalf { get; set; }

            [JsonProperty("T1")]
            public IList<Team> HomeTeam { get; set; }

            [JsonProperty("T2")]
            public IList<Team> AwayTeam { get; set; }

            [JsonProperty("Esd")]
            public string StartTime { get; set; }

            [JsonProperty("Edf")]
            public string EndTime { get; set; }
        }

        public class Team
        {
            [JsonProperty("Nm")]
            public string Name { get; set; }

            [JsonProperty("Img")]
            public string EmblemUrl { get; set; }

            [JsonProperty("CoNm")]
            public string Country { get; set; }
        }
    }
}
