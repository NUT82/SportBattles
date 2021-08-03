namespace SportBattles.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class Match : BaseDeletableModel<int>
    {
        public Match()
        {
            this.Games = new HashSet<GameMatch>();
        }

        public int HomeTeamId { get; set; }

        public virtual Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        public virtual Team AwayTeam { get; set; }

        public DateTime StartTime { get; set; }

        [MaxLength(10)]
        public string Status { get; set; }

        public byte? HomeGoals { get; set; }

        public byte? AwayGoals { get; set; }

        public byte? HomeGoalsFirstHalf { get; set; }

        public byte? AwayGoalsFirstHalf { get; set; }

        public int TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual ICollection<GameMatch> Games { get; set; }
    }
}
