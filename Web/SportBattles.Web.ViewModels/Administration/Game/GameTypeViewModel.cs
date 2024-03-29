﻿namespace SportBattles.Web.ViewModels.Administration.Game
{
    using System.Collections.Generic;

    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class GameTypeViewModel : IMapFrom<GameType>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<GamePointViewModel> GamePoints { get; set; }
    }
}
