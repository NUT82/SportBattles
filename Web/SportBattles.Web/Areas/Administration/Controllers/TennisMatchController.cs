namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using SportBattles.Common;
    using SportBattles.Services;
    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Administration.TennisMatch;

    public class TennisMatchController : AdministrationController
    {
        private readonly IConfiguration configuration;
        private readonly ILiveScoreApi liveScoreApi;
        private readonly ITennisMatchesService tennisMatchesService;
        private readonly DateTime startDateForResults = DateTime.UtcNow.AddDays(-3).AddHours(GlobalConstants.LiveScoreAPITimeZoneCorrection);
        private readonly DateTime yesterday = DateTime.UtcNow.AddDays(-1).AddHours(GlobalConstants.LiveScoreAPITimeZoneCorrection);

        public TennisMatchController(IConfiguration configuration, ILiveScoreApi liveScoreApi, ITennisMatchesService tennisMatchesService)
        {
            this.configuration = configuration;
            this.liveScoreApi = liveScoreApi;
            this.tennisMatchesService = tennisMatchesService;
        }

        public IActionResult AllInGame(int gameId)
        {
            var viewModel = new AllInGameViewModel
            {
                GameId = gameId,
                MatchesDoublePoints = this.tennisMatchesService.GetMatchesDoublePointsByGameId(gameId),
                Matches = this.tennisMatchesService.GetAllByGameId<MatchInGameViewModel>(gameId),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> DoublePoints(int matchId, int gameId)
        {
            await this.tennisMatchesService.ChangeDoublePoints(matchId, gameId);
            return this.RedirectToAction(nameof(this.AllInGame), new { gameId });
        }

        public async Task<IActionResult> GetAll()
        {
            await this.liveScoreApi.CreateJsonFilesForAllMatchesAsync(
                this.yesterday,
                this.yesterday.AddDays(GlobalConstants.LiveScoreAPIDaysAheadForTennis),
                this.configuration.GetValue<string>("X-RapidAPI-Key-Tennis"),
                this.configuration.GetValue<string>("X-RapidAPI-Host"),
                "Tennis");

            return this.RedirectToAction("Index", "Game");
        }

        public async Task<IActionResult> GetForDate(DateTime? date)
        {
            if (date is null)
            {
                throw new NullReferenceException("You must specify the date");
            }

            await this.liveScoreApi.CreateJsonFilesForAllMatchesAsync(
                date.Value,
                date.Value,
                this.configuration.GetValue<string>("X-RapidAPI-Key-Tennis"),
                this.configuration.GetValue<string>("X-RapidAPI-Host"),
                "Tennis");

            return this.RedirectToAction("Index", "Game");
        }

        public async Task<IActionResult> GetResults()
        {
            var matches = this.liveScoreApi.GetTennisMatches(this.startDateForResults, this.yesterday);

            await this.tennisMatchesService.PopulateResults(matches, this.startDateForResults, this.yesterday.AddDays(1));

            return this.RedirectToAction("Index", "Game");
        }
    }
}
