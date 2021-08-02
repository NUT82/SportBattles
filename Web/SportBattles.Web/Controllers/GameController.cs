namespace SportBattles.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Game;
    using SportBattles.Web.ViewModels.Home;

    public class GameController : BaseController
    {
        private readonly IGamesService gamesService;
        private readonly IMatchesService matchesService;
        private readonly IPredictionsService predictionsService;

        public GameController(IGamesService gamesService, IMatchesService matchesService, IPredictionsService predictionsService)
        {
            this.gamesService = gamesService;
            this.matchesService = matchesService;
            this.predictionsService = predictionsService;
        }

        [Authorize]
        public IActionResult MyGames()
        {
            var games = this.gamesService.GetUserGames<IndexGameViewModel>(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return this.View(games);
        }

        [Authorize]
        public IActionResult Predictions(int gameId)
        {
            var viewModel = new PredictionsViewModel
            {
                Matches = this.matchesService.GetAllByGameId<MatchInPredictionsViewModel>(gameId),
                GameId = gameId,
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

        [Authorize]
        public async Task<IActionResult> Join(int gameId)
        {
            await this.gamesService.Join(gameId, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return this.RedirectToAction(nameof(this.MyGames));
        }
    }
}
