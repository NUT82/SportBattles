namespace SportBattles.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class TennisPlayer : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}
