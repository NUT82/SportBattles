namespace SportBattles.Web.ViewModels.Administration.Sport
{
    using SportBattles.Services.JsonModels;
    using SportBattles.Services.Mapping;

    public class CreateGameViewModel : IMapFrom<FootballMatchJson>
    {
        public string EventsStatus { get; set; }

        public string EventsStartTime { get; set; }

        public string EventsHomeTeamName { get; set; }

        public string EventsAwayTeamName { get; set; }
    }
}
