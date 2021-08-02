namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Web.ViewModels.Game;

    public class PredictionsService : IPredictionsService
    {
        private readonly IDeletableEntityRepository<Prediction> predictionRepository;

        public PredictionsService(IDeletableEntityRepository<Prediction> predictionRepository)
        {
            this.predictionRepository = predictionRepository;
        }

        public async Task Add(PredictionInputModel input, string userId)
        {
            var prediction = this.predictionRepository.All().FirstOrDefault(p => p.GameId == input.GameId && p.MatchId == input.MatchId && p.UserId == userId);

            if (prediction == null)
            {
                prediction = new Prediction
                {
                    GameId = input.GameId,
                    MatchId = input.MatchId,
                    UserId = userId,
                };
                await this.predictionRepository.AddAsync(prediction);
            }

            prediction.HomeGoals = input.HomeGoals;
            prediction.AwayGoals = input.AwayGoals;

            await this.predictionRepository.SaveChangesAsync();
        }

        public IDictionary<int, PredictionViewModel> GetMatchesPredictions(int gameId, string userId)
        {
            return this.predictionRepository.AllAsNoTracking().Where(p => p.GameId == gameId && p.UserId == userId).Select(m => new KeyValuePair<int, PredictionViewModel>(m.MatchId, new PredictionViewModel { HomeGoals = m.HomeGoals, AwayGoals = m.AwayGoals })).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
