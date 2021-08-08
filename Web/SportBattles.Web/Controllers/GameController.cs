namespace SportBattles.Web.Controllers
{
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

        public GameController(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        [Authorize]
        public IActionResult MyGames()
        {
            var games = this.gamesService.GetUserGames<IndexGameViewModel>(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return this.View(games);
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
