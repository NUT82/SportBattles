namespace SportBattles.Web.ViewModels.Tennis
{
    using System.Collections.Generic;
    using System.Linq;

    public class MatchSheduleViewModel
    {
        public IEnumerable<MatchViewModel> Matches { get; set; }

        public IEnumerable<string> Tournaments => this.Matches.Select(m => m.TournamentCountryName + " - " + m.TournamentName).Distinct().ToList();
    }
}
