namespace SportBattles.Data.Models
{
    using System.Collections.Generic;

    using SportBattles.Data.Common.Models;

    public class SetGame : BaseDeletableModel<int>
    {
        public byte? HomeGames { get; set; }

        public byte? AwayGames { get; set; }

        public virtual ICollection<TennisMatch> TennisMatches { get; set; }
    }
}
