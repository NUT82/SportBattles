namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using SportBattles.Common;
    using SportBattles.Services;

    public class TennisController : AdministrationController
    {
        private readonly IConfiguration configuration;
        private readonly ILiveScoreApi liveScoreApi;

        private readonly DateTime startDateForResults = DateTime.UtcNow.AddDays(-3).AddHours(GlobalConstants.LiveScoreAPITimeZoneCorrection);
        private readonly DateTime yesterday = DateTime.UtcNow.AddDays(-1).AddHours(GlobalConstants.LiveScoreAPITimeZoneCorrection);

        public TennisController(IConfiguration configuration, ILiveScoreApi liveScoreApi)
        {
            this.configuration = configuration;
            this.liveScoreApi = liveScoreApi;
        }

        public async Task<IActionResult> GetAll()
        {
            await this.liveScoreApi.CreateJsonFilesForAllMatchesAsync(
                this.yesterday,
                this.yesterday.AddDays(GlobalConstants.LiveScoreAPIDaysAhead),
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
    }
}
