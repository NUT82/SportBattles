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
    using Xunit;

    public class TennisPlayersServiceTest
    {
        private Mock<IDeletableEntityRepository<TennisPlayer>> mockTennisPlayerRepository;
        private Mock<IDeletableEntityRepository<Country>> mockCountryRepository;
        private Mock<ITennisExplorerScraperService> mockTennisScraper;

        private TennisPlayersService service;

        public TennisPlayersServiceTest()
        {
            this.InitializeMapper();
            this.mockTennisPlayerRepository = new Mock<IDeletableEntityRepository<TennisPlayer>>();
            this.mockCountryRepository = new Mock<IDeletableEntityRepository<Country>>();
            this.mockTennisScraper = new Mock<ITennisExplorerScraperService>();

            this.service = new TennisPlayersService(this.mockTennisPlayerRepository.Object, this.mockCountryRepository.Object, this.mockTennisScraper.Object);
        }

        [Fact]
        public async Task AddMustAddPlayerWithGivenNameAndCountryToRepository()
        {
            var countryBG = new Country { Name = "Bulgaria" };
            var countryEN = new Country { Name = "England" };
            var player = new List<TennisPlayer>() { new TennisPlayer { Name = "Grisho", Id = 1, Country = countryBG }, new TennisPlayer { Name = "Nadal", Id = 2, Country = countryEN } };

            this.mockTennisPlayerRepository.Setup(x => x.All()).Returns(player.AsQueryable());
            this.mockTennisPlayerRepository.Setup(x => x.AddAsync(It.IsAny<TennisPlayer>())).Callback((TennisPlayer t) => player.Add(t));

            await this.service.Add("Federer", "Bulgaria");
            Assert.True(player.Count == 3);
            Assert.Contains(player, t => t.Name == "Federer");
        }

        [Fact]
        public async Task AddMustReturnTennisPlayerIdIfTennisPlayerAlreadyExist()
        {
            var countryBG = new Country { Name = "Bulgaria" };
            var countryEN = new Country { Name = "England" };
            var player = new List<TennisPlayer>() { new TennisPlayer { Name = "Grisho", Id = 1, Country = countryBG }, new TennisPlayer { Name = "Nadal", Id = 2, Country = countryEN } };

            this.mockTennisPlayerRepository.Setup(x => x.All()).Returns(player.AsQueryable());
            this.mockTennisPlayerRepository.Setup(x => x.AddAsync(It.IsAny<TennisPlayer>())).Callback((TennisPlayer t) => player.Add(t));

            var id = await this.service.Add("Grisho", "Bulgaria");
            Assert.True(player.Count == 2);
            Assert.True(id == 1);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("SportBattles.Web.ViewModels"));
    }
}
