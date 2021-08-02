﻿namespace SportBattles.Web.ViewModels.Game
{
    using System;

    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class MatchInPredictionsViewModel : IMapFrom<Match>
    {
        public int Id { get; set; }

        public string HomeTeamName { get; set; }

        public string HomeTeamEmblemUrl { get; set; }

        public string AwayTeamName { get; set; }

        public string AwayTeamEmblemUrl { get; set; }

        public DateTime StartTime { get; set; }

        public string StartTimeLocalTime => this.StartTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");

        public string TournamentName { get; set; }

        public string TournamentCountryName { get; set; }

        public byte? HomeGoals { get; set; }

        public byte? AwayGoals { get; set; }
    }
}
