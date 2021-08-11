namespace SportBattles.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Game;
    using SportBattles.Web.ViewModels.Tennis;

    public class TennisController : BaseController
    {
        private readonly ITennisMatchesService tennisMatchesService;
        private readonly ITennisPredictionsService tennisPredictionsService;
        private readonly IGamePointsService gamePointsService;

        public TennisController(ITennisMatchesService tennisMatchesService, ITennisPredictionsService tennisPredictionsService, IGamePointsService gamePointsService)
        {
            this.tennisMatchesService = tennisMatchesService;
            this.tennisPredictionsService = tennisPredictionsService;
            this.gamePointsService = gamePointsService;
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
            var viewModel = new PredictionsViewModel
            {
                Matches = this.tennisMatchesService.GetAllByGameId<MatchInPredictionsViewModel>(gameId),
                GameId = gameId,
                GamePoints = this.gamePointsService.GetAll<GamePointViewModel>(gameId),
                MatchesDoublePoints = this.tennisMatchesService.GetMatchesDoublePointsByGameId(gameId),
                MatchesPredictions = this.tennisPredictionsService.GetMatchesPredictions(gameId, this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
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

            if (inputModel.HomeSets == inputModel.AwaySets)
            {
                return this.Json(new { isValid = false, error = "A tennis match cannot end in a draw!" });
            }

            var matchStartTimeUTC = this.tennisMatchesService.GetStartTimeUTC(inputModel.MatchId);
            if (matchStartTimeUTC < DateTime.UtcNow)
            {
                return this.Json(new { isValid = false, error = "Too late - the match has already started!" });
            }

            await this.tennisPredictionsService.Add(inputModel, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return this.Json(new { isValid = true });
        }
    }
}
