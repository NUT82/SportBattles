namespace SportBattles.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Administration.Country;

    public class CountryController : AdministrationController
    {
        private readonly ICountriesService countriesService;

        public CountryController(ICountriesService countriesService)
        {
            this.countriesService = countriesService;
        }

        public JsonResult GetAllForSelectedSport([FromQuery] int sportId)
        {
            var countries = this.countriesService.GetAllForSport<CountriesViewModel>(sportId);
            return this.Json(countries);
        }

        public JsonResult GetAllOthersForSelectedSport([FromQuery] int sportId)
        {
            var countries = this.countriesService.GetAllOthersForSport<CountriesViewModel>(sportId);
            return this.Json(countries);
        }

        [HttpPost]
        public async Task<IActionResult> AddToSport([FromBody]AddCountryToSportInputModel inputModel)
        {
            var countryId = await this.countriesService.AddToSport(inputModel.CountryId, inputModel.SportId);

            return this.Json(countryId);
        }
    }
}
