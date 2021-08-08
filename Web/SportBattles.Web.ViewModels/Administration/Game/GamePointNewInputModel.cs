namespace SportBattles.Web.ViewModels.Administration.Game

{
    using System.ComponentModel.DataAnnotations;

    public class GamePointNewInputModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }
    }
}
