namespace SportBattles.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SportBattles.Data;
    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Data.Repositories;
    using SportBattles.Services.Mapping;
    using SportBattles.Web.ViewModels.Administration.Game;
    using Xunit;

    public class GamePointsServiceTest
    {
        public GamePointsServiceTest()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task AddMustAddNewGamePointByGivenNameAndDescription()
        {
            var mockRepoGamePoint = new Mock<IDeletableEntityRepository<GamePoint>>();
            var mockRepoGamePointGameType = new Mock<IRepository<GamePointGameType>>();
            IList<GamePoint> gamePoints = new List<GamePoint>
            {
                new GamePoint { Id = 1, Name = "Test", Description = "Test Description" },
            };
            mockRepoGamePoint.Setup(x => x.AddAsync(It.IsAny<GamePoint>())).Callback((GamePoint g) => gamePoints.Add(g));

            var service = new GamePointsService(mockRepoGamePoint.Object, mockRepoGamePointGameType.Object);

            await service.Add("Test2", "TestDescription2");
            Assert.True(gamePoints.Count == 2);
        }

        [Fact]
        public async Task GetAllMustGiveAllGamePoints()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            var dbContext = new ApplicationDbContext(options);

            dbContext.GamePoint.Add(new GamePoint { Id = 1, Name = "Test", Description = "Test Description" });
            await dbContext.SaveChangesAsync();

            var gamePointRepository = new EfDeletableEntityRepository<GamePoint>(dbContext);
            var gamePointGameTypeRepository = new EfRepository<GamePointGameType>(dbContext);

            var service = new GamePointsService(gamePointRepository, gamePointGameTypeRepository);

            var result = service.GetAll<GameAllPointViewModel>();
            Assert.True(result.Count() == 1);
            Assert.Contains(result, gp => gp.Name == "Test" && gp.Description == "Test Description");
        }

        [Fact]
        public async Task GetAllByGameIdMustGiveAllGamePointsInThisGame()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            var dbContext = new ApplicationDbContext(options);

            dbContext.GamePoint.Add(new GamePoint { Id = 1, Name = "Test", Description = "Test Description" });
            dbContext.GamePoint.Add(new GamePoint { Id = 2, Name = "Test2", Description = "Test Description2" });
            dbContext.Games.Add(new Game { Id = 1, GameType = new GameType { Id = 1, Name = "Test GameType" } });
            dbContext.Games.Add(new Game { Id = 2, GameType = new GameType { Id = 2, Name = "Test GameType2" } });
            dbContext.GamePointGameType.Add(new GamePointGameType { GamePointId = 1, GameTypeId = 1 });
            dbContext.GamePointGameType.Add(new GamePointGameType { GamePointId = 2, GameTypeId = 1 });

            await dbContext.SaveChangesAsync();

            var gamePointRepository = new EfDeletableEntityRepository<GamePoint>(dbContext);
            var gamePointGameTypeRepository = new EfRepository<GamePointGameType>(dbContext);

            var service = new GamePointsService(gamePointRepository, gamePointGameTypeRepository);

            var gameId = 1;
            var result = service.GetAll<GamePointViewModel>(gameId);
            Assert.True(result.Count() == 2);
            Assert.Contains(result, gp => gp.GamePointName == "Test" && gp.GamePointDescription == "Test Description");

            gameId = 2;
            result = service.GetAll<GamePointViewModel>(gameId);
            Assert.True(result.Count() == 0);
        }

        [Fact]
        public async Task IsDuplicateNameMustReturnTrueIfNameAlreadyExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            var dbContext = new ApplicationDbContext(options);

            dbContext.GamePoint.Add(new GamePoint { Id = 1, Name = "Test", Description = "Test Description" });
            await dbContext.SaveChangesAsync();

            var gamePointRepository = new EfDeletableEntityRepository<GamePoint>(dbContext);
            var gamePointGameTypeRepository = new EfRepository<GamePointGameType>(dbContext);

            var service = new GamePointsService(gamePointRepository, gamePointGameTypeRepository);

            var result = service.IsDuplicateName("Test");
            Assert.True(result);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("SportBattles.Web.ViewModels"));
    }
}
