namespace SportBattles.Web.ViewModels.Administration.Game

{
    using System.ComponentModel.DataAnnotations;

    public class GamePointInputModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(0, 10)]
        public byte Value { get; set; }
    }
}
