namespace SportBattles.Web.Components
{
    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.User;

    [ViewComponent(Name ="TopPlayers")]
    public class TopPlayersViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public TopPlayersViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IViewComponentResult Invoke()
        {
            var model = this.usersService.GetTopPlayers<TopPlayersComponentViewModel>(5);
            return this.View(model);
        }
    }
}
