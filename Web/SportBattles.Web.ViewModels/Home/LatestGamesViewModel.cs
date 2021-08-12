namespace SportBattles.Web.ViewModels.Home
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class LatestGamesViewModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GameTypeName { get; set; }

        public string DisplayName => this.GameTypeName + " - " + this.Name;

        public string SportName => this.GameTypeName?.Split()[0];

        public int UsersCount { get; set; }

        public int MatchesCount { get; set; }

        public int TennisMatchesCount { get; set; }
    }
}
