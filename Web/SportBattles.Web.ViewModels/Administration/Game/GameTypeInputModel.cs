namespace SportBattles.Web.ViewModels.Administration.Game
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class GameTypeInputModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name field must be with a minimum length of 3 characters.")]
        [MaxLength(50, ErrorMessage = "Name field must be with a maximum length of 50 characters.")]
        public string Name { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Description field must be with a minimum length of 10 characters.")]
        public string Description { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<GamePointInputModel> SelectedScoringPoints { get; set; }
    }
}
