namespace SportBattles.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Home;

    public class GameController : BaseController
    {
        private readonly IGamesService gamesService;

        public GameController(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var games = this.gamesService.GetAllStarted<GameViewModel>(userId);
            return this.View(games);
        }

        [Authorize]
        public IActionResult MyGames()
        {
            var model = new IndexGameViewModel
            {
                GameViewModel = this.gamesService.GetUserGames<GameViewModel>(this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                UnpredictedMatches = this.gamesService.UnpredictedMatchesInGameByUserCount(this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Ranking(int gameId)
        {
            var ranking = this.gamesService.GetRanking(gameId);
            return this.View(ranking);
        }

        [Authorize]
        public async Task<IActionResult> Join(int gameId)
        {
            await this.gamesService.Join(gameId, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return this.RedirectToAction(nameof(this.MyGames));
        }
    }
}
