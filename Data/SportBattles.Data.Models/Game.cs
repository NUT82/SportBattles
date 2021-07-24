namespace SportBattles.Data.Models
{
    using System.Collections.Generic;

    using SportBattles.Data.Common.Models;

    public class Game : BaseDeletableModel<int>
    {
        public Game()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Matches = new HashSet<Match>();
        }

        public string Name { get; set; }

        public int GameTypeId { get; set; }

        public GameType GameType { get; set; }

        public bool IsFinished { get; set; } = false;

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
