namespace SportBattles.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using SportBattles.Data.Common.Models;

    [Index(nameof(Name), IsUnique = true)]
    public class GameType : BaseDeletableModel<int>
    {
        public GameType()
        {
            this.Games = new HashSet<Game>();
            this.GamePoints = new HashSet<GamePointGameType>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<GamePointGameType> GamePoints { get; set; }
    }
}
