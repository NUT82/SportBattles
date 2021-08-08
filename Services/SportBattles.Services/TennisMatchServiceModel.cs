namespace SportBattles.Services
{
    using System;
    using System.Collections.Generic;

    public class TennisMatchServiceModel
    {
        public TennisMatchServiceModel()
        {
            this.Games = new List<Game>();
        }

        public string Id { get; set; }

        public string Status { get; set; }

        public string Country { get; set; }

        public string Tournament { get; set; }

        public DateTime StartTimeUTC { get; set; }

        public string StartTimeLocalTime => this.StartTimeUTC.ToLocalTime().ToString("dd.MM.yyyy HH:mm");

        public string HomeTeam { get; set; }

        public string HomeTeamCountry { get; set; }

        public string AwayTeam { get; set; }

        public string AwayTeamCountry { get; set; }

        public byte? HomeSets { get; set; }

        public byte? AwaySets { get; set; }

        public ICollection<Game> Games { get; set; }

        public class Game
        {
            public byte? HomeGames { get; set; }

            public byte? AwayGames { get; set; }
        }
    }
}
