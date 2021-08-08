namespace SportBattles.Web.ViewModels.Tennis
{
    using System.Collections.Generic;

    using SportBattles.Web.ViewModels.Game;

    public class PredictionsViewModel
    {
        public int GameId { get; set; }

        public IEnumerable<MatchInPredictionsViewModel> Matches { get; set; }

        public IEnumerable<GamePointViewModel> GamePoints { get; set; }

        public IDictionary<int, PredictionViewModel> MatchesPredictions { get; set; }

        public IDictionary<int, bool> MatchesDoublePoints { get; set; }
    }
}
