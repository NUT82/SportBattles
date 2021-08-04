﻿namespace SportBattles.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using SportBattles.Data.Common.Models;

    [Index(nameof(Name), IsUnique = true)]
    public class GameType : BaseDeletableModel<int>
    {
        public GameType()
        {
            this.GamePoints = new HashSet<GamePoint>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<GamePoint> GamePoints { get; set; }

        ////public byte ExactScorelinePoints { get; set; }

        ////public byte GoalDifferencePoints { get; set; }

        ////public byte OneTeamGoalsPoints { get; set; }

        ////public byte OutcomePoints { get; set; }
    }
}
