namespace SportBattles.Data.Models
{
    public class GameTennisMatch
    {
        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public int TennisMatchId { get; set; }

        public virtual TennisMatch TennisMatch { get; set; }

        public bool DoublePoints { get; set; } = false;
    }
}
