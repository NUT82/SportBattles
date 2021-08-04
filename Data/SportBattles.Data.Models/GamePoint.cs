namespace SportBattles.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using SportBattles.Data.Common.Models;

    [Index(nameof(Name), IsUnique = true)]
    public class GamePoint : BaseDeletableModel<int>
    {
        public GamePoint()
        {
            this.GameTypes = new HashSet<GameType>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public byte Value { get; set; }

        public virtual ICollection<GameType> GameTypes { get; set; }
    }
}
