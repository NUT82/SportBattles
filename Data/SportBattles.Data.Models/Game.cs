namespace SportBattles.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class Game : BaseDeletableModel<int>
    {
        public Game()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Matches = new HashSet<GameMatch>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int GameTypeId { get; set; }

        public virtual GameType GameType { get; set; }

        public bool Started { get; set; } = false;

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<GameMatch> Matches { get; set; }
    }
}
