namespace SportBattles.Web.ViewModels.Home
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class IndexGameViewModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GameTypeName { get; set; }

        public string GameTypeSport => this.GameTypeName?.Split()[0];

        public int MatchesCount { get; set; }

        public int TennisMatchesCount { get; set; }

        public int UsersCount { get; set; }
    }
}
