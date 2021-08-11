namespace SportBattles.Web.ViewModels.Game
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class TopGamesComponentViewModel : IMapFrom<Game>
    {
        public string Name { get; set; }

        public string GameTypeName { get; set; }

        public string DisplayName => this.GameTypeName + " - " + this.Name;

        public int UsersCount { get; set; }

        public int MatchesCount { get; set; }

        public int TennisMatchesCount { get; set; }

        public int AllCount => this.MatchesCount + this.TennisMatchesCount;
    }
}
