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
                var points = this.CalculatePoints(prediction.HomeGoals, prediction.AwayGoals, prediction.Match.HomeGoals, prediction.Match.AwayGoals, prediction.Game.GameType, doublePoints);
                prediction.Points = (byte?)points;
            }

            await this.predictionRepository.SaveChangesAsync();
        }

        private int? CalculatePoints(byte? predictionHomeGoals, byte? predictionAwayGoals, byte? homeGoals, byte? awayGoals, GameType gameType, bool doublePoints)
        {
            if (predictionHomeGoals is null || predictionAwayGoals is null || homeGoals is null || awayGoals is null)
            {
                return null;
            }

            var multiplier = doublePoints ? 2 : 1;

            if (predictionHomeGoals == homeGoals && predictionAwayGoals == awayGoals)
            {
                return gameType.GamePoints.FirstOrDefault(p => p.Name == "ExactScorelinePoints").Value * multiplier;
            }

            if (predictionHomeGoals - predictionAwayGoals == homeGoals - awayGoals)
            {
                return gameType.GamePoints.FirstOrDefault(p => p.Name == "GoalDifferencePoints").Value * multiplier;
            }

            var predictionOutcome = predictionHomeGoals - predictionAwayGoals;
            var resultOutcome = homeGoals - awayGoals;
            var outcome = (predictionOutcome == 0 && resultOutcome == 0) || (predictionOutcome * resultOutcome > 0);

            if (outcome && (predictionHomeGoals == homeGoals || predictionAwayGoals == awayGoals))
            {
                return gameType.GamePoints.FirstOrDefault(p => p.Name == "OneTeamGoalsPoints").Value * multiplier;
            }

            if (outcome)
            {
                return gameType.GamePoints.FirstOrDefault(p => p.Name == "OutcomePoints").Value * multiplier;
            }

            return 0;
        }
    }
}
