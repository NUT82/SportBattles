namespace SportBattles.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Web.ViewModels.Game;

    public class PredictionsService : IPredictionsService
    {
        private readonly IDeletableEntityRepository<Prediction> predictionRepository;
        private readonly IMatchesService matchesService;

        public PredictionsService(IDeletableEntityRepository<Prediction> predictionRepository, IMatchesService matchesService)
        {
            this.predictionRepository = predictionRepository;
            this.matchesService = matchesService;
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
            return this.predictionRepository.AllAsNoTracking().Where(p => p.GameId == gameId && p.UserId == userId).Select(m => new KeyValuePair<int, PredictionViewModel>(m.MatchId, new PredictionViewModel { HomeGoals = m.HomeGoals, AwayGoals = m.AwayGoals, Points = m.Points })).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public async Task PopulatePoints()
        {
            var predictions = this.predictionRepository.All().Where(p => p.Points == null && p.Match.StartTime < DateTime.UtcNow && p.HomeGoals != null).ToList();
            foreach (var prediction in predictions)
            {
                var doublePoints = this.matchesService.IsDoublePoint(prediction.GameId, prediction.MatchId);
                var points = this.CalculatePoints(prediction);
                prediction.Points = (byte?)points;
            }

            await this.predictionRepository.SaveChangesAsync();
        }

        private int? CalculatePoints(Prediction prediction)
        {
            if (prediction.HomeGoals is null || prediction.AwayGoals is null || prediction.Match.HomeGoals is null || prediction.Match.AwayGoals is null)
            {
                return null;
            }

            var multiplier = this.matchesService.IsDoublePoint(prediction.GameId, prediction.MatchId) ? 2 : 1;
            var maxResult = 0;
            foreach (var gamePoint in prediction.Game.GameType.GamePoints)
            {
                var result = gamePoint.Name switch
                {
                    "Exact scoreline" => this.GetPoints(prediction, gamePoint, multiplier, this.ExactScorelinePoints()),
                    "Goal difference" => this.GetPoints(prediction, gamePoint, multiplier, this.GoalDifferencePoints()),
                    "Goals scored by one of the teams" => this.GetPoints(prediction, gamePoint, multiplier, this.OneTeamGoalsPoints()),
                    "Outcome" => this.GetPoints(prediction, gamePoint, multiplier, this.OutcomePoints()),
                    _ => throw new ArgumentException("Add method to calculate points for this GamePointType first!"),
                };

                if (result > maxResult)
                {
                    maxResult = result;
                }
            }

            return maxResult;
        }



        private int GetPoints(Prediction prediction, GamePoint gamePoint, int multiplier, Func<Prediction, bool> condition)
        {
            if (condition(prediction))
            {
                return gamePoint.Value * multiplier;
            }

            return 0;
        }

        private Func<Prediction, bool> ExactScorelinePoints() => (prediction) =>
            prediction.HomeGoals == prediction.Match.HomeGoals &&
            prediction.AwayGoals == prediction.Match.AwayGoals;

        private Func<Prediction, bool> GoalDifferencePoints() => (prediction) =>
            prediction.HomeGoals - prediction.AwayGoals == prediction.Match.HomeGoals - prediction.Match.AwayGoals;

        private Func<Prediction, bool> OneTeamGoalsPoints() => (prediction) =>
            (this.IsCorrectWinner(prediction) &&
            (prediction.HomeGoals == prediction.Match.HomeGoals || prediction.AwayGoals == prediction.Match.AwayGoals));

        private Func<Prediction, bool> OutcomePoints() => (prediction) =>
            this.IsCorrectWinner(prediction);

        private bool IsCorrectWinner(Prediction prediction)
        {
            var predictionOutcome = prediction.HomeGoals - prediction.AwayGoals;
            var resultOutcome = prediction.Match.HomeGoals - prediction.Match.AwayGoals;
            var outcome = (predictionOutcome == 0 && resultOutcome == 0) || (predictionOutcome * resultOutcome > 0);
            return outcome;
        }
    }
}
