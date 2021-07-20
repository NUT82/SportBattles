namespace SportBattles.Web.ViewModels.Administration.Tournament
{
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class TournamentsViewModel : IMapFrom<Tournament>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
