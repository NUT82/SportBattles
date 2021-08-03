namespace SportBattles.Web.ViewModels.Administration.Game
{
    using System.ComponentModel.DataAnnotations;

    public class GameTypeInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        [RegularExpression("[0-9]{1}")]
        public byte ExactScorelinePoints { get; set; }

        [Required]
        [RegularExpression("[0-9]{1}")]
        public byte GoalDifferencePoints { get; set; }

        [Required]
        [RegularExpression("[0-9]{1}")]
        public byte OneTeamGoalsPoints { get; set; }

        [Required]
        [RegularExpression("[0-9]{1}")]
        public byte OutcomePoints { get; set; }
    }
}
