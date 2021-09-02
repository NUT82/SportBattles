namespace SportBattles.Web.ViewModels.Football
{
    using System;

    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class MatchesViewModel : IMapFrom<GameMatch>
    {
        public string MatchHomeTeamName { get; set; }

        public string MatchHomeTeamEmblemUrl { get; set; }

        public string MatchAwayTeamName { get; set; }

        public string MatchAwayTeamEmblemUrl { get; set; }

        public DateTime MatchStartTime { get; set; }

        public string MatchStartTimeISO => this.MatchStartTime.ToString("s");

        public string MatchTournamentName { get; set; }

        public string MatchTournamentCountryName { get; set; }
    }
}
