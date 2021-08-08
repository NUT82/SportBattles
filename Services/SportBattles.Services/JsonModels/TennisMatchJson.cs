namespace SportBattles.Services.JsonModels
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class TennisMatchJson
    {
        [JsonProperty("Snm")]
        public string Tournament { get; set; }

        [JsonProperty("Cnm")]
        public string Country { get; set; }

        public IList<Event> Events { get; set; }

        public class Event
        {
            [JsonProperty("Eid")]
            public string Id { get; set; }

            [JsonProperty("Eps")]
            public string Status { get; set; }

            [JsonProperty("Tr1")]
            public string HomeSets { get; set; }

            [JsonProperty("Tr2")]
            public string AwaySets { get; set; }

            [JsonProperty("Tr1S1")]
            public string HomeGamesSet1 { get; set; }

            [JsonProperty("Tr2S1")]
            public string AwayGamesSet1 { get; set; }

            [JsonProperty("Tr1S2")]
            public string HomeGamesSet2 { get; set; }

            [JsonProperty("Tr2S2")]
            public string AwayGamesSet2 { get; set; }

            [JsonProperty("Tr1S3")]
            public string HomeGamesSet3 { get; set; }

            [JsonProperty("Tr2S3")]
            public string AwayGamesSet3 { get; set; }

            [JsonProperty("Tr1S4")]
            public string HomeGamesSet4 { get; set; }

            [JsonProperty("Tr2S4")]
            public string AwayGamesSet4 { get; set; }

            [JsonProperty("Tr1S5")]
            public string HomeGamesSet5 { get; set; }

            [JsonProperty("Tr2S5")]
            public string AwayGamesSet5 { get; set; }

            [JsonProperty("T1")]
            public IList<Team> Home { get; set; }

            [JsonProperty("T2")]
            public IList<Team> Away { get; set; }

            [JsonProperty("Esd")]
            public string StartTime { get; set; }
        }

        public class Team
        {
            [JsonProperty("Nm")]
            public string Name { get; set; }

            [JsonProperty("CoNm")]
            public string Country { get; set; }
        }
    }
}
