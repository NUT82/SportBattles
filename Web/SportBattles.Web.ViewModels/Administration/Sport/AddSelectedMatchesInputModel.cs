namespace SportBattles.Web.ViewModels.Administration.Sport
{
    using System.Collections.Generic;

    using SportBattles.Services;

    public class AddSelectedMatchesInputModel
    {
        public IEnumerable<FootballMatch> Matches { get; set; }

        public int GameId { get; set; }
    }
}
