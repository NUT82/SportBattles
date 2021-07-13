namespace SportBattles.Web.Areas.Administration.Controllers
{
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
    }
}
