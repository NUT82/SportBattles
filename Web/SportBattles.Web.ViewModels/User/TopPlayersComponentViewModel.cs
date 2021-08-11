namespace SportBattles.Web.ViewModels.User
{
    using System.Linq;

    using AutoMapper;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class TopPlayersComponentViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public int Points { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, TopPlayersComponentViewModel>().ForMember(m => m.Points, opt => opt.MapFrom(u => u.Predictions.Sum(p => p.Points) + u.TennisPredictions.Sum(p => p.Points)));
        }
    }
}
