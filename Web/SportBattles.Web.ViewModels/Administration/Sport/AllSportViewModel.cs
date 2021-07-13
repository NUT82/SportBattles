namespace SportBattles.Web.ViewModels.Administration.Sport
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class AllSportViewModel : IMapFrom<Sport>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
