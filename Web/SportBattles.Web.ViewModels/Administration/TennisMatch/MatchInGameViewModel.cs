namespace SportBattles.Web.ViewModels.Administration.TennisMatch
{
    using System;
    using System.Collections.Generic;

    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class MatchInGameViewModel : IMapFrom<TennisMatch>
    {
        public int Id { get; set; }

        public string HomePlayerName { get; set; }

        public string HomePlayerPictureUrl { get; set; }

        public string AwayPlayerName { get; set; }

        public string AwayPlayerPictureUrl { get; set; }

        public DateTime StartTime { get; set; }

        public string StartTimeLocalTime => this.StartTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");

        public string Status { get; set; }

        public string TournamentName { get; set; }

        public string TournamentCountryName { get; set; }

        public byte? HomeSets { get; set; }

        public byte? AwaySets { get; set; }

        public ICollection<SetGame> SetGames { get; set; }
    }
}
