namespace SportBattles.Data.Models
{
    public class GamePointGameType
    {
        public int GamePointId { get; set; }

        public virtual GamePoint GamePoint { get; set; }

        public int GameTypeId { get; set; }

        public virtual GameType GameType { get; set; }

        public byte Value { get; set; }
    }
}
