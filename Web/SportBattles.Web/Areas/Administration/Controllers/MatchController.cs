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

        private readonly DateTime startDateForResults = DateTime.UtcNow.AddDays(-3).AddHours(GlobalConstants.LiveScoreAPITimeZoneCorrection);
        private readonly DateTime yesterday = DateTime.UtcNow.AddDays(-1).AddHours(GlobalConstants.LiveScoreAPITimeZoneCorrection);

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
            await this.liveScoreApi.CreateJsonFilesForAllMatchesAsync(
                this.yesterday,
                this.yesterday.AddDays(GlobalConstants.LiveScoreAPIDaysAheadForFootball),
                this.configuration.GetValue<string>("X-RapidAPI-Key"),
                this.configuration.GetValue<string>("X-RapidAPI-Host"),
                "Soccer");

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
                this.configuration.GetValue<string>("X-RapidAPI-Key"),
                this.configuration.GetValue<string>("X-RapidAPI-Host"),
                "Soccer");

            return this.RedirectToAction("Index", "Game");
        }

        public async Task<IActionResult> GetResults()
        {
            var matches = this.liveScoreApi.GetFootballMatches(this.startDateForResults, this.yesterday);

            await this.matchesService.PopulateResults(matches, this.startDateForResults, this.yesterday.AddDays(1));

            return this.RedirectToAction("Index", "Game");
        }

        public JsonResult Show(string sport, string country, string tournament)
        {
            return sport switch
            {
                "Football" => this.Json(
                    this.liveScoreApi.GetFootballMatches(
                       this.yesterday.AddDays(1),
                       this.yesterday.AddDays(GlobalConstants.LiveScoreAPIDaysAheadForFootball),
                       country,
                       tournament)),
                "Tennis" => this.Json(
                    this.liveScoreApi.GetTennisMatches(
                       this.yesterday.AddDays(1),
                       this.yesterday.AddDays(GlobalConstants.LiveScoreAPIDaysAheadForFootball),
                       country,
                       tournament)),
                _ => throw new ArgumentOutOfRangeException("This sport is not supported"),
            };
        }
    }
}
