namespace SportBattles.Data.Models
{
    public class GameMatch
    {
        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public int MatchId { get; set; }

        public virtual Match Match { get; set; }

        public bool DoublePoints { get; set; } = false;
    }
}
