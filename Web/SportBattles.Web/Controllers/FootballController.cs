namespace SportBattles.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Football;

    public class FootballController : BaseController
    {
        private readonly IMatchesService matchesService;
        private readonly IPredictionsService predictionsService;

        public FootballController(IMatchesService matchesService, IPredictionsService predictionsService)
        {
            this.matchesService = matchesService;
            this.predictionsService = predictionsService;
        }

        [Authorize]
        public IActionResult Predictions(int gameId)
        {
            var viewModel = new PredictionsViewModel
            {
                Matches = this.matchesService.GetAllByGameId<MatchInPredictionsViewModel>(gameId),
                GameId = gameId,
                MatchesDoublePoints = this.matchesService.GetMatchesDoublePointsByGameId(gameId),
                MatchesPredictions = this.predictionsService.GetMatchesPredictions(gameId, this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SavePredictions([FromBody] PredictionInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, error = "Error saving match result!" });
            }

            var matchStartTimeUTC = this.matchesService.GetStartTimeUTC(inputModel.MatchId);
            if (matchStartTimeUTC < DateTime.UtcNow)
            {
                return this.Json(new { isValid = false, error = "Too late - the match has already started!" });
            }

            await this.predictionsService.Add(inputModel, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return this.Json(new { isValid = true });
        }
    }
}
