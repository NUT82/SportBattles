namespace SportBattles.Web.ViewModels.Game
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class GamePointViewModel : IMapFrom<GamePointGameType>
    {
        public string GamePointName { get; set; }

        public string GamePointDescription { get; set; }

        public byte Value { get; set; }

        public string NameValue => $"{this.GamePointName} - {this.Value}";
    }
}
