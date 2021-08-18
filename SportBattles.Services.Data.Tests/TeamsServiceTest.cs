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
    using Xunit;

    public class TeamsServiceTest
    {
        private Mock<IDeletableEntityRepository<Team>> mockTeamRepository;
        private Mock<IDeletableEntityRepository<Country>> mockCountryRepository;

        private TeamsService service;

        public TeamsServiceTest()
        {
            this.InitializeMapper();
            this.mockTeamRepository = new Mock<IDeletableEntityRepository<Team>>();
            this.mockCountryRepository = new Mock<IDeletableEntityRepository<Country>>();
            this.service = new TeamsService(this.mockTeamRepository.Object, this.mockCountryRepository.Object);
        }

        [Fact]
        public async Task AddMustAddTeamWithGivenNameEmblemAndCountryToRepository()
        {
            var countryBG = new Country { Name = "Bulgaria" };
            var countryEN = new Country { Name = "England" };
            var teams = new List<Team>() { new Team { Name = "Test Team 1", Id = 1, Country = countryBG, EmblemUrl = "emblem1" }, new Team { Name = "Test Team 2", Id = 2, Country = countryEN, EmblemUrl = "emblem2" } };

            this.mockTeamRepository.Setup(x => x.All()).Returns(teams.AsQueryable());
            this.mockTeamRepository.Setup(x => x.AddAsync(It.IsAny<Team>())).Callback((Team t) => teams.Add(t));

            await this.service.Add("Test Team 3", "emblem3", "Bulgaria");
            Assert.True(teams.Count == 3);
            Assert.Contains(teams, t => t.Name == "Test Team 3");
        }

        [Fact]
        public async Task AddMustReturnTeamIdIfTeamAlreadyExist()
        {
            var countryBG = new Country { Name = "Bulgaria" };
            var countryEN = new Country { Name = "England" };
            var teams = new List<Team>() { new Team { Name = "Test Team 1", Id = 1, Country = countryBG, EmblemUrl = "emblem1" }, new Team { Name = "Test Team 2", Id = 2, Country = countryEN, EmblemUrl = "emblem2" } };

            this.mockTeamRepository.Setup(x => x.All()).Returns(teams.AsQueryable());
            this.mockTeamRepository.Setup(x => x.AddAsync(It.IsAny<Team>())).Callback((Team t) => teams.Add(t));

            var id = await this.service.Add("Test Team 2", "emblem2", "England");
            Assert.True(teams.Count == 2);
            Assert.True(id == 2);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("SportBattles.Web.ViewModels"));
    }
}
