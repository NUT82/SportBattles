namespace SportBattles.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Web.ViewModels.Tennis;

    public class TennisPredictionsService : ITennisPredictionsService
    {
        private readonly IDeletableEntityRepository<TennisPrediction> tennisPredictionRepository;
        private readonly ITennisMatchesService tennisMatchesService;

        public TennisPredictionsService(IDeletableEntityRepository<TennisPrediction> tennisPredictionRepository, ITennisMatchesService tennisMatchesService)
        {
            this.tennisPredictionRepository = tennisPredictionRepository;
            this.tennisMatchesService = tennisMatchesService;
        }

        public async Task Add(PredictionInputModel input, string userId)
        {
            var prediction = this.tennisPredictionRepository.All().FirstOrDefault(p => p.GameId == input.GameId && p.TennisMatchId == input.MatchId && p.UserId == userId);

            if (prediction == null)
            {
                prediction = new TennisPrediction
                {
                    GameId = input.GameId,
                    TennisMatchId = input.MatchId,
                    UserId = userId,
                };
                await this.tennisPredictionRepository.AddAsync(prediction);
            }

            prediction.HomeSets = input.HomeSets;
            prediction.AwaySets = input.AwaySets;

            await this.tennisPredictionRepository.SaveChangesAsync();
        }

        public IDictionary<int, PredictionViewModel> GetMatchesPredictions(int gameId, string userId)
        {
            return this.tennisPredictionRepository.AllAsNoTracking().Where(p => p.GameId == gameId && p.UserId == userId).Select(m => new KeyValuePair<int, PredictionViewModel>(m.TennisMatchId, new PredictionViewModel { HomeSets = m.HomeSets, AwaySets = m.AwaySets, Points = m.Points })).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public async Task PopulatePoints()
        {
            var predictions = this.tennisPredictionRepository.All().Where(p => p.Points == null && p.TennisMatch.StartTime < DateTime.UtcNow && p.HomeSets != null).ToList();
            foreach (var prediction in predictions)
            {
                var points = this.CalculatePoints(prediction);
                prediction.Points = (byte?)points;
            }

            await this.tennisPredictionRepository.SaveChangesAsync();
        }

        private int? CalculatePoints(TennisPrediction prediction)
        {
            if (prediction.HomeSets is null || prediction.AwaySets is null || prediction.TennisMatch.HomeSets is null || prediction.TennisMatch.AwaySets is null)
            {
                return null;
            }

            if (prediction.TennisMatch.Status != "FT")
            {
                return 0;
            }

            var multiplier = this.tennisMatchesService.IsDoublePoint(prediction.GameId, prediction.TennisMatchId) ? 2 : 1;
            var maxResult = 0;
            foreach (var gamePoint in prediction.Game.GameType.GamePoints)
            {
                var result = gamePoint.GamePoint.Name switch
                {
                    "Exact sets" => this.GetPoints(prediction, gamePoint.Value, multiplier, this.ExactScorelinePoints()),
                    "Tennis player winner" => this.GetPoints(prediction, gamePoint.Value, multiplier, this.OutcomePoints()),
                    _ => throw new ArgumentException("Add method to calculate points for this GamePointType first!"),
                };

                if (result > maxResult)
                {
                    maxResult = result;
                }
            }

            return maxResult;
        }

        private int GetPoints(TennisPrediction prediction, byte value, int multiplier, Func<TennisPrediction, bool> condition)
        {
            if (condition(prediction))
            {
                return value * multiplier;
            }

            return 0;
        }

        private Func<TennisPrediction, bool> ExactScorelinePoints() => (prediction) =>
            prediction.HomeSets == prediction.TennisMatch.HomeSets &&
            prediction.AwaySets == prediction.TennisMatch.AwaySets;

        private Func<TennisPrediction, bool> OutcomePoints() => (prediction) =>
           this.IsCorrectWinner(prediction);

        private bool IsCorrectWinner(TennisPrediction prediction)
        {
            var predictionOutcome = prediction.HomeSets - prediction.AwaySets;
            var resultOutcome = prediction.TennisMatch.HomeSets - prediction.TennisMatch.AwaySets;
            var outcome = (predictionOutcome == 0 && resultOutcome == 0) || (predictionOutcome * resultOutcome > 0);
            return outcome;
        }
    }
}
