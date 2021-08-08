namespace SportBattles.Web.ViewModels.Tennis
{
    using System.ComponentModel.DataAnnotations;

    public class PredictionInputModel
    {
        [Required]
        [RegularExpression("[0-3]{1}")]
        public byte HomeSets { get; set; }

        [Required]
        [RegularExpression("[0-3]{1}")]
        public byte AwaySets { get; set; }

        [Required]
        public int GameId { get; set; }

        [Required]
        public int MatchId { get; set; }
    }
}
