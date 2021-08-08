namespace SportBattles.Web.ViewModels.Football
{
    using System.Collections.Generic;

    public class PredictionsViewModel
    {
        public int GameId { get; set; }

        public IEnumerable<MatchInPredictionsViewModel> Matches { get; set; }

        public IDictionary<int, PredictionViewModel> MatchesPredictions { get; set; }

        public IDictionary<int, bool> MatchesDoublePoints { get; set; }
    }
}
