namespace SportBattles.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class Country : BaseDeletableModel<int>
    {
        public Country()
        {
            this.Sports = new HashSet<Sport>();
            this.Tournaments = new HashSet<Tournament>();
        }

        [Required]
        [MaxLength(45)]
        public string Name { get; set; }

        [MaxLength(6)]
        public string Code { get; set; }

        public string FlagUrl { get; set; }

        public virtual ICollection<Sport> Sports { get; set; }

        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}
