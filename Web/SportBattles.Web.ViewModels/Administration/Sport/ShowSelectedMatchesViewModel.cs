namespace SportBattles.Web.ViewModels.Administration.Sport
{
    using System.Collections.Generic;

    using SportBattles.Services;

    public class ShowSelectedMatchesViewModel
    {
        public IEnumerable<FootballMatch> Matches { get; set; }
    }
}
