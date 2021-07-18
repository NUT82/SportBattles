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

        public IActionResult Add()
        {
            return this.View();
        }

        public async Task<IActionResult> GetMatches()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://livescore6.p.rapidapi.com/matches/v2/list-by-date?Category=soccer&Date=20210718"),
                Headers =
                {
                    { "x-rapidapi-key", this.configuration.GetValue<string>("X-RapidAPI-Key") },
                    { "x-rapidapi-host", this.configuration.GetValue<string>("X-RapidAPI-Host") },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                using FileStream fileStream = System.IO.File.Create(@"wwwroot/json/data.json");
                await response.Content.CopyToAsync(fileStream);
            }

            return this.View();
        }

        public IActionResult ShowMatches()
        {
            var jsonFilePath = @"wwwroot/json/data.json";
            var matches = this.liveScoreApi.GetFootballMatches(jsonFilePath);

            return this.View(matches);
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
    }
}
