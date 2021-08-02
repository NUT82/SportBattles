namespace SportBattles.Web.ViewModels.Game
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PredictionInputModel
    {
        [Required]
        [RegularExpression("[0-9]{1}")]
        public byte HomeGoals { get; set; }

        [Required]
        [RegularExpression("[0-9]{1}")]
        public byte AwayGoals { get; set; }

        [Required]
        public int GameId { get; set; }

        [Required]
        public int MatchId { get; set; }
    }
}
