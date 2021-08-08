namespace SportBattles.Web.ViewModels.Game
{
    using System.Collections.Generic;

    using SportBattles.Data.Models;

    public class RankingViewModel
    {
        public string UserName { get; set; }

        public string ProfilePictureId { get; set; }

        public ICollection<GamePoint> GamePoints { get; set; }

        public int Points { get; set; }
    }
}
