namespace SportBattles.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;
    using SportBattles.Data.Models.Enums;

    public class Prediction : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public int MatchId { get; set; }

        public virtual Match Match { get; set; }

        public byte? HomeGoals { get; set; }

        public byte? AwayGoals { get; set; }

        public ThreeWayResult? ThreeWayWinner { get; set; }

        public byte? Points { get; set; }
    }
}
