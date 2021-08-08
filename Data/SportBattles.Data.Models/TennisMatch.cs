namespace SportBattles.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class TennisMatch : BaseDeletableModel<int>
    {
        public TennisMatch()
        {
            this.Games = new HashSet<GameTennisMatch>();
        }

        public int HomePlayerId { get; set; }

        public virtual TennisPlayer HomePlayer { get; set; }

        public int AwayPlayerId { get; set; }

        public virtual TennisPlayer AwayPlayer { get; set; }

        public DateTime StartTime { get; set; }

        [MaxLength(10)]
        public string Status { get; set; }

        public byte? HomeSets { get; set; }

        public byte? AwaySets { get; set; }

        public int TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual ICollection<GameTennisMatch> Games { get; set; }

        public virtual ICollection<SetGame> SetGames { get; set; }
    }
}
