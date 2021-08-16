namespace SportBattles.Web.ViewModels.Tennis
{
    using System;
    using System.Collections.Generic;

    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class MatchInPredictionsViewModel : IMapFrom<TennisMatch>
    {
        public int Id { get; set; }

        public string HomePlayerName { get; set; }

        public string HomePlayerPictureUrl { get; set; }

        public string AwayPlayerName { get; set; }

        public string AwayPlayerPictureUrl { get; set; }

        public DateTime StartTime { get; set; }

        public string StartTimeISO => this.StartTime.ToString("s");

        public string Status { get; set; }

        public string TournamentName { get; set; }

        public string TournamentCountryName { get; set; }

        public byte? HomeSets { get; set; }

        public byte? AwaySets { get; set; }

        public ICollection<SetGame> SetGames { get; set; }
    }
}
