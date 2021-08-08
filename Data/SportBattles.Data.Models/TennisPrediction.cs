namespace SportBattles.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;
    using SportBattles.Data.Models.Enums;

    public class TennisPrediction : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public int TennisMatchId { get; set; }

        public virtual TennisMatch TennisMatch { get; set; }

        public byte? HomeSets { get; set; }

        public byte? AwaySets { get; set; }

        public TwoWayResult? TwoWayWinner { get; set; }

        public byte? Points { get; set; }
    }
}
