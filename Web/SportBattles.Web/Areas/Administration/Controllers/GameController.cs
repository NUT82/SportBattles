namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Administration.Game;
    using SportBattles.Web.ViewModels.Administration.Sport;

    public class GameController : AdministrationController
    {
        private readonly IGamesService gamesService;

        public GameController(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = this.gamesService.GetAll<GameViewModel>();
            return this.View(games);
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddGameInputModel inputModel)
        {
            if (this.ModelState.IsValid)
            {
                if (this.gamesService.IsDuplicate(inputModel.Game.Name, inputModel.Game.TypeID))
                {
                    this.ModelState.AddModelError(string.Empty, "Duplicate name of game of this type");
                    return this.View(inputModel);
                }
                else
                {
                    await this.gamesService.Add(inputModel.Game.Name, inputModel.Game.TypeID);
                    return this.RedirectToAction(nameof(this.Index));
                }
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddSelectedMatches([FromBody] AddSelectedMatchesInputModel inputModel)
        {
            await this.gamesService.AddMatches(inputModel.GameId, inputModel.Matches);
            return this.Json(new { redirectToUrl = this.Url.Action("Index", "Game") });
        }

        public async Task<IActionResult> ChangeStatus(int gameId)
        {
            await this.gamesService.ChangeStatus(gameId);
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int gameId)
        {
            await this.gamesService.Delete(gameId);
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddGameType([FromBody] GameTypeInputModel inputModel)
        {
            if (this.ModelState.IsValid)
            {
                if (this.gamesService.IsDuplicateTypeName(inputModel.Name))
                {
                    this.ModelState.AddModelError(string.Empty, "Duplicate name of game type");
                    return this.View(nameof(this.Add), inputModel);
                }
                else
                {
                    await this.gamesService.AddType(inputModel);
                    return this.Json(inputModel);
                }
            }

            return this.View(nameof(this.Add), inputModel);
        }

        public JsonResult GetAllGameTypes()
        {
            var gameTypes = this.gamesService.GetAllTypes<GameTypeViewModel>();
            return this.Json(gameTypes);
        }
    }
}
