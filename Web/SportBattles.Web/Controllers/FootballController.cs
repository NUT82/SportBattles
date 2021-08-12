namespace SportBattles.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels;
    using SportBattles.Web.ViewModels.Football;
    using SportBattles.Web.ViewModels.Game;

    public class FootballController : BaseController
    {
        private readonly IMatchesService matchesService;
        private readonly IPredictionsService predictionsService;
        private readonly IGamePointsService gamePointsService;
        private readonly IGamesService gamesService;

        public FootballController(IMatchesService matchesService, IPredictionsService predictionsService, IGamePointsService gamePointsService, IGamesService gamesService)
        {
            this.matchesService = matchesService;
            this.predictionsService = predictionsService;
            this.gamePointsService = gamePointsService;
            this.gamesService = gamesService;
        }

        public IActionResult Schedule()
        {
            return this.View();
        }

        public IActionResult Results()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Predictions(int gameId)
        {
            if (!this.gamesService.IsUserInGame(this.User.FindFirstValue(ClaimTypes.NameIdentifier), gameId))
            {
                return this.View("Error", new ErrorViewModel { ErrorMsg = "You are not in this game!" });
            }

            var viewModel = new PredictionsViewModel
            {
                Matches = this.matchesService.GetAllByGameId<MatchInPredictionsViewModel>(gameId),
                GameId = gameId,
                GamePoints = this.gamePointsService.GetAll<GamePointViewModel>(gameId),
                MatchesDoublePoints = this.matchesService.GetMatchesDoublePointsByGameId(gameId),
                MatchesPredictions = this.predictionsService.GetMatchesPredictions(gameId, this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SavePredictions([FromBody] PredictionInputModel inputModel)
        {
            if (!this.gamesService.IsUserInGame(this.User.FindFirstValue(ClaimTypes.NameIdentifier), inputModel.GameId))
            {
                return this.View("Error", new ErrorViewModel { ErrorMsg = "You are not in this game!" });
            }

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
