namespace SportBattles.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class Sport : BaseDeletableModel<int>
    {
        public Sport()
        {
            this.Countries = new HashSet<Country>();
        }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public virtual ICollection<Country> Countries { get; set; }
    }
}
