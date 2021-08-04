namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;

    public class PredictionController : AdministrationController
    {
        private readonly IPredictionsService predictionsService;

        public PredictionController(IPredictionsService predictionsService)
        {
            this.predictionsService = predictionsService;
        }

        public async Task<IActionResult> GetPoints()
        {
            await this.predictionsService.PopulatePoints();

            return this.RedirectToAction("Index", "Game");
        }
    }
}
