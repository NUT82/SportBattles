namespace SportBattles.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class Tournament : BaseDeletableModel<int>
    {
        public Tournament()
        {
            this.Teams = new HashSet<Team>();
            this.Matches = new HashSet<Match>();
        }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        public int SportId { get; set; }

        public Sport Sport { get; set; }

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
