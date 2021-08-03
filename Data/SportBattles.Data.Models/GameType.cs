namespace SportBattles.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class GameType : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public byte ExactScorelinePoints { get; set; }

        public byte GoalDifferencePoints { get; set; }

        public byte OneTeamGoalsPoints { get; set; }

        public byte OutcomePoints { get; set; }
    }
}
