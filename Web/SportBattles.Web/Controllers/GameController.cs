namespace SportBattles.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels;
    using SportBattles.Web.ViewModels.Game;

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

        public IActionResult Info(int id)
        {
            var game = this.gamesService.GetGame<GameInfoViewModel>(id);
            if (game == null)
            {
                return this.View("Error", new ErrorViewModel { ErrorMsg = "This game doesn't exists!" });
            }

            return this.View(game);
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
        public async Task<IActionResult> Join(int gameId, string sport)
        {
            await this.gamesService.Join(gameId, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return this.RedirectToAction("Predictions", sport, new { gameId });
        }
    }
}
