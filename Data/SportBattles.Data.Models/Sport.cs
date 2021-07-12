namespace SportBattles.Data.Models
{
    using System.Collections.Generic;

    using SportBattles.Data.Common.Models;

    public class Sport : BaseDeletableModel<int>
    {
        public Sport()
        {
            this.Countries = new HashSet<Country>();
        }

        public string Name { get; set; }

        public virtual ICollection<Country> Countries { get; set; }
    }
}
