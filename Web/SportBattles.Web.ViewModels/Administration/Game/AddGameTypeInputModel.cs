namespace SportBattles.Web.ViewModels.Administration.Game
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddGameTypeInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }
    }
}
