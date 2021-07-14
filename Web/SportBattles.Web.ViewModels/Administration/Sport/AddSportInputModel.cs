namespace SportBattles.Web.ViewModels.Administration.Sport
{
    using System.ComponentModel.DataAnnotations;

    public class AddSportInputModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
