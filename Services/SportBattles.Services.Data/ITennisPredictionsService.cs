﻿namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SportBattles.Web.ViewModels.Tennis;

    public interface ITennisPredictionsService
    {
        public Task Add(PredictionInputModel input, string userId);

        public IDictionary<int, PredictionViewModel> GetMatchesPredictions(int gameId, string userId);

        public Task PopulatePoints();
    }
}
