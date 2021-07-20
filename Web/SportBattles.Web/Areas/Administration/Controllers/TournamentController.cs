namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Administration.Tournament;

    public class TournamentController : AdministrationController
    {
        private readonly ITournamentsService tournamentsService;

        public TournamentController(ITournamentsService tournamentsService)
        {
            this.tournamentsService = tournamentsService;
        }

        public JsonResult GetTournamentsForSelectedSportInCountry([FromQuery] int sportId, [FromQuery] int countryId)
        {
            var tournaments = this.tournamentsService.GetTournamentsForSportInCountry<TournamentsViewModel>(sportId, countryId);
            return this.Json(tournaments);
        }

        [HttpPost]
        public async Task<IActionResult> AddTournamentToSportInCountry([FromBody] TournamentsViewModel inputModel)
        {
            ////TODO
            await this.tournamentsService.AddNewTournamentToSportInCountry(1, 2, "not implement");
            return this.Json(inputModel);
        }
    }
}
