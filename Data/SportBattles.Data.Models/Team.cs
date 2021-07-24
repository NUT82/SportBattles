namespace SportBattles.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class Team : BaseDeletableModel<int>
    {
        public Team()
        {
            this.Tournaments = new HashSet<Tournament>();
        }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        public string EmblemUrl { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}
