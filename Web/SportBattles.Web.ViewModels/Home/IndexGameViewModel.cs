namespace SportBattles.Web.ViewModels.Home
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class IndexGameViewModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GameTypeName { get; set; }

        public int MatchesCount { get; set; }

        public int UsersCount { get; set; }
    }
}
