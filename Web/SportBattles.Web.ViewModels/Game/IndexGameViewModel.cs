namespace SportBattles.Web.ViewModels.Game
{
    using System.Collections.Generic;

    public class IndexGameViewModel
    {
        public IEnumerable<GameViewModel> GameViewModel { get; set; }

        public IDictionary<int, int> UnpredictedMatches { get; set; }
    }
}
