﻿namespace SportBattles.Web.ViewModels.Administration.Match
{
    using System.Collections.Generic;

    public class AllInGameViewModel
    {
        public int GameId { get; set; }

        public IDictionary<int, bool> MatchesDoublePoints { get; set; }

        public IEnumerable<MatchInGameViewModel> Matches { get; set; }
    }
}
