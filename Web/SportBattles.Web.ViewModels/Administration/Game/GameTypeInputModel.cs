namespace SportBattles.Web.ViewModels.Administration.Game
{
    using System.ComponentModel.DataAnnotations;

    public class GameTypeInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string TypeName { get; set; }

        [Required]
        [MinLength(10)]
        public string TypeDescription { get; set; }
    }
}
