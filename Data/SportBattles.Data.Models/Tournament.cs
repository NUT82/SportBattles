namespace SportBattles.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class Tournament : BaseDeletableModel<int>
    {
        public Tournament()
        {
            this.Matches = new HashSet<Match>();
        }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        public int SportId { get; set; }

        public virtual Sport Sport { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
