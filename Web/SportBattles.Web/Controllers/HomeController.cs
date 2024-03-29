﻿namespace SportBattles.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels;
    using SportBattles.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IGamesService gamesService;

        public HomeController(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = this.gamesService.GetLatest<LatestGamesViewModel>();
            return this.View(games);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { ErrorMsg = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
