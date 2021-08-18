namespace SportBattles.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Moq;
    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;
    using SportBattles.Services.TennisPlayerPictureScraper;
    using SportBattles.Web.ViewModels.Administration.Game;
    using SportBattles.Web.ViewModels.Administration.Sport;
    using SportBattles.Web.ViewModels.Game;
    using SportBattles.Web.ViewModels.Home;
    using Xunit;

    public class SportsServiceTest
    {
        private Mock<IDeletableEntityRepository<Sport>> mockSportRepository;
        private SportsService service;

        public SportsServiceTest()
        {
            this.InitializeMapper();
            this.mockSportRepository = new Mock<IDeletableEntityRepository<Sport>>();
            this.service = new SportsService(this.mockSportRepository.Object);
        }

        [Fact]
        public async Task AddMustAddSportByGivenNameSuccesfull()
        {
            var sports = new List<Sport>();
            this.mockSportRepository.Setup(x => x.AddAsync(It.IsAny<Sport>())).Callback((Sport s) => sports.Add(s));
            this.mockSportRepository.Setup(x => x.All()).Returns(sports.AsQueryable());

            await this.service.Add("Test Sport");

            Assert.True(sports.Count == 1);
            Assert.Contains(sports, s => s.Name == "Test Sport");

            await this.service.Add("Test Sport");
            Assert.True(sports.Count == 1);
        }

        [Fact]
        public void GetAllMustReturnAllSports()
        {
            var sports = new List<Sport>()
            {
                new Sport { Id = 1, Name = "Test Sport" },
                new Sport { Id = 2, Name = "Test Sport2" },
                new Sport { Id = 3, Name = "Test Sport3" },
            };
            this.mockSportRepository.Setup(x => x.All()).Returns(sports.AsQueryable());

            var result = this.service.GetAll<AllSportViewModel>();
            Assert.True(sports.Count == 3);
        }

        private void InitializeMapper() => AutoMapperConfig.
          RegisterMappings(Assembly.Load("SportBattles.Web.ViewModels"));
    }
}
