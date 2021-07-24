namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Administration.Game;

    public class GameController : AdministrationController
    {
        private readonly IGamesService gamesService;

        public GameController(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddGameTypeInputModel gameType)
        {
            if (this.ModelState.IsValid)
            {
                if (this.gamesService.DuplicateName(gameType.Name))
                {
                    this.ModelState.AddModelError(nameof(AddGameTypeInputModel.Name), "Duplicate name of game type");
                    return this.ValidationProblem();
                }
                else
                {
                    await this.gamesService.Add(gameType.Name, gameType.Description);
                    return this.Json(gameType);
                }
            }

            return this.View(gameType);
        }

        public JsonResult GetAllGameTypes()
        {
            var gameTypes = this.gamesService.GetAll<GameTypeViewModel>();
            return this.Json(gameTypes);
        }
    }
}
