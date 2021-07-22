namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using SportBattles.Services;
    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Administration.Sport;

    public class SportController : AdministrationController
    {
        private readonly ISportsService sportsService;
        private readonly IConfiguration configuration;
        private readonly ILiveScoreApi liveScoreApi;

        public SportController(ISportsService sportsService, IConfiguration configuration, ILiveScoreApi liveScoreApi)
        {
            this.sportsService = sportsService;
            this.configuration = configuration;
            this.liveScoreApi = liveScoreApi;
        }

        public IActionResult CreateGame()
        {
            return this.View();
        }

        public IActionResult Games()
        {
            return this.View();
        }

        public async Task<IActionResult> GetMatches()
        {
            await this.liveScoreApi.CreateJsonFilesForAllFootballMatchesAsync(
                DateTime.Today,
                DateTime.Today.AddDays(6),
                this.configuration.GetValue<string>("X-RapidAPI-Key"),
                this.configuration.GetValue<string>("X-RapidAPI-Host"));

            return this.View();
        }

        public IActionResult ShowMatches(string country, string tournament)
        {
            var matches = this.liveScoreApi.GetFootballMatches(DateTime.Today, DateTime.Today.AddDays(6), country, tournament);

            return this.Json(matches);
        }

        public JsonResult GetAllSports()
        {
            var allSports = this.sportsService.GetAll<AllSportViewModel>();
            return this.Json(allSports);
        }

        [HttpPost]
        public async Task<IActionResult> AddSport([FromBody] AddSportInputModel sportName)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem();
            }

            await this.sportsService.AddNewSport(sportName.Name);

            return this.Json(sportName);
        }

        [HttpPost]
        public IActionResult ShowSelectedMatches([FromBody] ShowSelectedMatchesViewModel data)
        {
            ////TODO: add to current game
            return this.RedirectToAction(nameof(this.Games));
        }
    }
}
