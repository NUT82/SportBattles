namespace SportBattles.Data.Models
{
    using System.Collections.Generic;

    using SportBattles.Data.Common.Models;

    public class Country : BaseDeletableModel<int>
    {
        public Country()
        {
            this.Sports = new HashSet<Sport>();
            this.Tournaments = new HashSet<Tournament>();
        }

        public string Name { get; set; }

        public string FlagId { get; set; }

        public Image Flag { get; set; }

        public virtual ICollection<Sport> Sports { get; set; }

        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}
