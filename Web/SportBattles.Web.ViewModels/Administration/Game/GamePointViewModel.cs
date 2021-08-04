namespace SportBattles.Web.ViewModels.Administration.Game
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class GamePointViewModel : IMapFrom<GamePoint>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte Value { get; set; }
    }
}
