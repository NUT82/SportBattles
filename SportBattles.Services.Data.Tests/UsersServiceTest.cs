namespace SportBattles.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Moq;
    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;
    using SportBattles.Services.TennisPlayerPictureScraper;
    using SportBattles.Web.ViewModels.User;
    using Xunit;

    public class UsersServiceTest
    {
        private Mock<IDeletableEntityRepository<ApplicationUser>> mockUserRepository;

        private UsersService service;

        public UsersServiceTest()
        {
            this.InitializeMapper();
            this.mockUserRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();

            this.service = new UsersService(this.mockUserRepository.Object);
        }

        [Fact]
        public void GetTopPlayersMustReturnCountPlayersSortedByPoints()
        {
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1", UserName = "Test UserName", Predictions = new List<Prediction>()
                        {
                            new Prediction { Points = 5 },
                            new Prediction { Points = 3 },
                            new Prediction { Points = 2 },
                        },
                },
                new ApplicationUser
                {
                    Id = "2", UserName = "Test UserName2", Predictions = new List<Prediction>()
                        {
                            new Prediction { Points = 1 },
                            new Prediction { Points = 1 },
                            new Prediction { Points = 1 },
                        },
                },
                new ApplicationUser
                {
                    Id = "3", UserName = "Test UserName3", Predictions = new List<Prediction>()
                        {
                            new Prediction { Points = 2 },
                            new Prediction { Points = 2 },
                            new Prediction { Points = 2 },
                        },
                },
                new ApplicationUser
                {
                    Id = "4", UserName = "Test UserName4", Predictions = new List<Prediction>()
                        {
                            new Prediction { Points = 3 },
                            new Prediction { Points = 3 },
                            new Prediction { Points = 3 },
                        },
                },
            };

            this.mockUserRepository.Setup(x => x.AllAsNoTracking()).Returns(users.AsQueryable());

            var result = this.service.GetTopPlayers<TopPlayersComponentViewModel>(3);
            Assert.True(result.Count() == 3);
            Assert.True(result.First().UserName == "Test UserName");
            Assert.True(result.Last().UserName == "Test UserName3");
        }

        [Fact]
        public void GetTopPlayersInGameMustReturnCountPlayersInThisGameSortedByPoints()
        {
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1", Games = new List<Game>() { new Game { Id = 2 } }, UserName = "Test UserName", Predictions = new List<Prediction>()
                        {
                            new Prediction { Points = 5 },
                            new Prediction { Points = 3 },
                            new Prediction { Points = 2 },
                        },
                },
                new ApplicationUser
                {
                    Id = "2", Games = new List<Game>() { new Game { Id = 1 } }, UserName = "Test UserName2", Predictions = new List<Prediction>()
                        {
                            new Prediction { Points = 1 },
                            new Prediction { Points = 1 },
                            new Prediction { Points = 1 },
                        },
                },
                new ApplicationUser
                {
                    Id = "3", Games = new List<Game>() { new Game { Id = 1 } }, UserName = "Test UserName3", Predictions = new List<Prediction>()
                        {
                            new Prediction { Points = 2 },
                            new Prediction { Points = 2 },
                            new Prediction { Points = 2 },
                        },
                },
                new ApplicationUser
                {
                    Id = "4", Games = new List<Game>() { new Game { Id = 2 } }, UserName = "Test UserName4", Predictions = new List<Prediction>()
                        {
                            new Prediction { Points = 3 },
                            new Prediction { Points = 3 },
                            new Prediction { Points = 3 },
                        },
                },
            };

            this.mockUserRepository.Setup(x => x.AllAsNoTracking()).Returns(users.AsQueryable().OrderByDescending(u => u.Predictions.Sum(p => p.Points )));

            var result = this.service.GetTopPlayersInGame<TopPlayersComponentViewModel>(1, 2);
            Assert.True(result.Count() == 2);
            Assert.True(result.First().UserName == "Test UserName3");
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("SportBattles.Web.ViewModels"));
    }
}
