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

        public JsonResult GetAll()
        {
            var allSports = this.sportsService.GetAll<AllSportViewModel>();
            return this.Json(allSports);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddSportInputModel sportName)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem();
            }

            await this.sportsService.Add(sportName.Name);

            return this.Json(sportName);
        }
    }
}
