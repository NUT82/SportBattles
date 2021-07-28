namespace SportBattles.Web.ViewModels.Administration.Game
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class GameViewModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GameTypeName { get; set; }

        public int MatchesCount { get; set; }

        public int UsersCount { get; set; }

        public bool Started { get; set; }
    }
}
