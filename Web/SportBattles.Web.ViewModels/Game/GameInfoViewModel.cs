namespace SportBattles.Web.ViewModels.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;
    using SportBattles.Web.ViewModels.Football;
    using SportBattles.Web.ViewModels.Tennis;

    public class GameInfoViewModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GameTypeName { get; set; }

        public ICollection<GamePointViewModel> GameTypeGamePoints { get; set; }

        public string GameTypeSport => this.GameTypeName.Split()[0];

        public string GameTypeDescription { get; set; }

        public ICollection<MatchesViewModel> Matches { get; set; }

        public ICollection<MatchesViewModel> NotStartedMatches => this.Matches.Where(m => m.MatchStartTime > DateTime.UtcNow).OrderBy(m => m.MatchStartTime).ToList();

        public ICollection<TennisMatchesViewModel> TennisMatches { get; set; }

        public ICollection<TennisMatchesViewModel> NotStartedTennisMatches => this.TennisMatches.Where(m => m.TennisMatchStartTime > DateTime.UtcNow).OrderBy(m => m.TennisMatchStartTime).ToList();
    }
}
