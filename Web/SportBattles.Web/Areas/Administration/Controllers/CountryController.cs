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

        [HttpPost]
        public async Task<IActionResult> AddCountryToSport([FromBody]CountriesViewModel countryName, int sportId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem();
            }

            await this.countriesService.AddNewCountryToSport(countryName.Name, sportId);

            return this.Json(countryName);
        }
    }
}
