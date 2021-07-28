namespace SportBattles.Services
{
    using System;

    public class FootballMatch
    {
        public string Id { get; set; }

        public string Status { get; set; }

        public string Country { get; set; }

        public string Tournament { get; set; }

        public DateTime StartTimeUTC { get; set; }

        public string StartTimeLocalTime => this.StartTimeUTC.ToLocalTime().ToString("dd.MM.yyyy HH:mm");

        public string HomeTeam { get; set; }

        public string HomeTeamCountry { get; set; }

        public string HomeTeamEmblemUrl { get; set; }

        public string AwayTeam { get; set; }

        public string AwayTeamCountry { get; set; }

        public string AwayTeamEmblemUrl { get; set; }

        public byte? HomeGoals { get; set; }

        public byte? AwayGoals { get; set; }

        public byte? HalfHomeGoals { get; set; }

        public byte? HalfAwayGoals { get; set; }
    }
}
