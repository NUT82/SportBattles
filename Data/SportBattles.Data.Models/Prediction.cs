namespace SportBattles.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;
    using SportBattles.Data.Models.Enums;

    public class Prediction : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public int MatchId { get; set; }

        public Match Match { get; set; }

        public int? HomeGoals { get; set; }

        public int? AwayGoals { get; set; }

        public ThreeWayResult? ThreeWayWinner { get; set; }
    }
}
