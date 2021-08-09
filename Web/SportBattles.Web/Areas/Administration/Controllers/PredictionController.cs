namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;

    public class PredictionController : AdministrationController
    {
        private readonly IPredictionsService predictionsService;
        private readonly ITennisPredictionsService tennisPredictionsService;

        public PredictionController(IPredictionsService predictionsService, ITennisPredictionsService tennisPredictionsService)
        {
            this.predictionsService = predictionsService;
            this.tennisPredictionsService = tennisPredictionsService;
        }

        public async Task<IActionResult> GetPoints()
        {
            await this.predictionsService.PopulatePoints();
            await this.tennisPredictionsService.PopulatePoints();

            return this.RedirectToAction("Index", "Game");
        }
    }
}
