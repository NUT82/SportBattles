namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using SportBattles.Common;
    using SportBattles.Services;
    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Administration.Match;

    public class MatchController : AdministrationController
    {
        private readonly IConfiguration configuration;
        private readonly ILiveScoreApi liveScoreApi;
        private readonly IMatchesService matchesService;

        public MatchController(IConfiguration configuration, ILiveScoreApi liveScoreApi, IMatchesService matchesService)
        {
            this.configuration = configuration;
            this.liveScoreApi = liveScoreApi;
            this.matchesService = matchesService;
        }

        public IActionResult AllInGame(int gameId)
        {
            var viewModel = new AllInGameViewModel
            {
                GameId = gameId,
                MatchesDoublePoints = this.matchesService.GetMatchesDoublePointsByGameId(gameId),
                Matches = this.matchesService.GetAllByGameId<MatchInGameViewModel>(gameId),
            };

            return this.View(viewModel);
        }

        public IActionResult AddToGame(int gameId)
        {
            return this.View(gameId);
        }

        public async Task<IActionResult> DoublePoints(int matchId, int gameId)
        {
            await this.matchesService.ChangeDoublePoints(matchId, gameId);
            return this.RedirectToAction(nameof(this.AllInGame), new { gameId });
        }

        public async Task<IActionResult> GetAll()
        {
            await this.liveScoreApi.CreateJsonFilesForAllFootballMatchesAsync(
                DateTime.Today.AddDays(-1),
                DateTime.Today.AddDays(GlobalConstants.LiveScoreAPIDaysAhead),
                this.configuration.GetValue<string>("X-RapidAPI-Key"),
                this.configuration.GetValue<string>("X-RapidAPI-Host"));

            return this.RedirectToAction("Index", "Game");
        }

        public JsonResult Show(string country, string tournament)
        {
            var matches = this.liveScoreApi.GetFootballMatches(
                DateTime.Today,
                DateTime.Today.AddDays(GlobalConstants.LiveScoreAPIDaysAhead),
                country,
                tournament);

            return this.Json(matches);
        }
    }
}
