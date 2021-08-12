namespace SportBattles.Web.Components
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using SportBattles.Services.Data;

    [ViewComponent(Name ="TopPlayersInGame")]
    public class TopPlayersInGameViewComponent : ViewComponent
    {
        private readonly IGamesService gamesService;

        public TopPlayersInGameViewComponent(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        public IViewComponentResult Invoke(int gameId)
        {
            var model = this.gamesService.GetRanking(gameId).Take(5);
            return this.View(model);
        }
    }
}
