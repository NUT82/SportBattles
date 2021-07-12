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
        public string Name { get; set; }

        public string EmblemId { get; set; }

        public Image Emblem { get; set; }

        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}
