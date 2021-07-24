namespace SportBattles.Data.Models
{
    using System;
    using System.Collections.Generic;

    using SportBattles.Data.Common.Models;

    public class Match : BaseDeletableModel<int>
    {
        public Match()
        {
            this.Games = new HashSet<Game>();
        }

        public int HomeTeamId { get; set; }

        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        public Team AwayTeam { get; set; }

        public DateTime StartTime { get; set; }

        public byte? HomeGoals { get; set; }

        public byte? AwayGoals { get; set; }

        public byte? HomeGoalsFirstHalf { get; set; }

        public byte? AwayGoalsFirstHalf { get; set; }

        public int TournamentId { get; set; }

        public Tournament Tournament { get; set; }

        public byte DifficultyMultiplier { get; set; } = 1;

        public virtual ICollection<Game> Games { get; set; }
    }
}
