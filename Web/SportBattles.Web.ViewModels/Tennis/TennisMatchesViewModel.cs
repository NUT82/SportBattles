namespace SportBattles.Web.ViewModels.Tennis
{
    using System;
    using System.Collections.Generic;

    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class TennisMatchesViewModel : IMapFrom<GameTennisMatch>
    {
        public string TennisMatchHomePlayerName { get; set; }

        public string TennisMatchHomePlayerPictureUrl { get; set; }

        public string TennisMatchAwayPlayerName { get; set; }

        public string TennisMatchAwayPlayerPictureUrl { get; set; }

        public DateTime TennisMatchStartTime { get; set; }

        public string TennisMatchStartTimeLocalTime => this.TennisMatchStartTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");

        public string TennisMatchTournamentName { get; set; }

        public string TennisMatchTournamentCountryName { get; set; }
    }
}
