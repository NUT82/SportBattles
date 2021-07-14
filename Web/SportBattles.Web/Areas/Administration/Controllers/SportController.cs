namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Administration.Sport;

    public class SportController : AdministrationController
    {
        private readonly ISportsService sportsService;

        public SportController(ISportsService sportsService)
        {
            this.sportsService = sportsService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        public JsonResult GetAllSports()
        {
            var allSports = this.sportsService.GetAll<AllSportViewModel>();
            return this.Json(allSports);
        }

        [HttpPost]
        public async Task<IActionResult> AddSport([FromBody]AddSportInputModel sportName)
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
