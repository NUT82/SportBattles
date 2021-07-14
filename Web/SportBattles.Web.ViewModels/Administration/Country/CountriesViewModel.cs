namespace SportBattles.Web.ViewModels.Administration.Country
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class CountriesViewModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
