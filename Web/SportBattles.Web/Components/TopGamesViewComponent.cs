namespace SportBattles.Web.Components
{
    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Services.Data;
    using SportBattles.Web.ViewModels.Game;

    [ViewComponent(Name ="TopGames")]
    public class TopGamesViewComponent : ViewComponent
    {
        private readonly IGamesService gamesService;

        public TopGamesViewComponent(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        public IViewComponentResult Invoke()
        {
            var model = this.gamesService.GetTopGames<TopGamesComponentViewModel>(3);
            return this.View(model);
        }
    }
}
