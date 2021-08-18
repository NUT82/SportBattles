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
    using SportBattles.Web.ViewModels.Administration.Tournament;
    using Xunit;

    public class TournamentsServiceTest
    {
        private Mock<IDeletableEntityRepository<Tournament>> mockTournamentRepository;

        private TournamentsService service;

        public TournamentsServiceTest()
        {
            this.InitializeMapper();
            this.mockTournamentRepository = new Mock<IDeletableEntityRepository<Tournament>>();

            this.service = new TournamentsService(this.mockTournamentRepository.Object);
        }

        [Fact]
        public void GetAllForSportInCountryMustReturnAllTournamentsInThisSportAndThisCountry()
        {
            var tournaments = new List<Tournament>()
            {
                new Tournament { Id = 1, Name = "Test Tournament1", SportId = 1, CountryId = 1 },
                new Tournament { Id = 2, Name = "Test Tournament2", SportId = 2, CountryId = 2 },
                new Tournament { Id = 3, Name = "Test Tournament3", SportId = 3, CountryId = 3 },
            };
            this.mockTournamentRepository.Setup(x => x.AllAsNoTracking()).Returns(tournaments.AsQueryable());

            var result = this.service.GetAllForSportInCountry<TournamentsViewModel>(2, 2);
            Assert.True(result.Count() == 1);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("SportBattles.Web.ViewModels"));
    }
}
