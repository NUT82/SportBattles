namespace SportBattles.Web.ViewModels.Administration.Game
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class GameTypeViewModel : IMapFrom<GameType>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
