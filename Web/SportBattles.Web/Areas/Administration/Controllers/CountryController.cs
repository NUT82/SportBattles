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

        public JsonResult GetCountriesForSelectedSport([FromQuery] int sportId)
        {
            var countries = this.countriesService.GetCountriesForSport<CountriesViewModel>(sportId);
            return this.Json(countries);
        }

        public JsonResult GetAllOtherCountriesForSelectedSport([FromQuery] int sportId)
        {
            var countries = this.countriesService.GetAllOtherCountriesForSport<CountriesViewModel>(sportId);
            return this.Json(countries);
        }

        [HttpPost]
        public async Task<IActionResult> AddCountryToSport([FromBody]AddCountryToSportInputModel inputModel)
        {
            var countryId = await this.countriesService.AddNewCountryToSport(inputModel.CountryId, inputModel.SportId);

            return this.Json(countryId);
        }
    }
}
