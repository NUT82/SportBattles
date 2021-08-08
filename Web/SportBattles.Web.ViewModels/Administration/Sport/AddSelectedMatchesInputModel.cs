namespace SportBattles.Web.ViewModels.Administration.Sport
{
    using System.Collections.Generic;

    using SportBattles.Services;

    public class AddSelectedMatchesInputModel
    {
        public IEnumerable<FootballMatchServiceModel> Matches { get; set; }

        public IEnumerable<TennisMatchServiceModel> TennisMatches { get; set; }

        public int GameId { get; set; }

        public string Sport { get; set; }
    }
}
