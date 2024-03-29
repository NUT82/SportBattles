﻿namespace SportBattles.Web.Areas.Administration.Controllers
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

        public JsonResult GetAllForSelectedSportInCountry([FromQuery] int sportId, [FromQuery] int countryId)
        {
            var tournaments = this.tournamentsService.GetAllForSportInCountry<TournamentsViewModel>(sportId, countryId);
            return this.Json(tournaments);
        }
    }
}
