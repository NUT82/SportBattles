namespace SportBattles.Web.ViewModels.Administration.Game
{
    using System.ComponentModel.DataAnnotations;

    public class GameInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int TypeID { get; set; }
    }
}
