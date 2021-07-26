namespace SportBattles.Web.ViewModels.Administration.Game
{
    using System.ComponentModel.DataAnnotations;

    public class AddGameInputModel
    {
        public GameInputModel Game { get; set; }

        public GameTypeInputModel GameType { get; set; }
    }
}
