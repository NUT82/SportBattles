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
    using SportBattles.Web.ViewModels.Administration.Match;
    using Xunit;

    using Match = SportBattles.Data.Models.Match;

    public class MatchesServiceTest
    {
        private Mock<IDeletableEntityRepository<Match>> mockMatchesRepository;
        private Mock<IRepository<GameMatch>> mockGameMatchRepository;
        private MatchesService service;

        public MatchesServiceTest()
        {
            this.InitializeMapper();
            this.mockMatchesRepository = new Mock<IDeletableEntityRepository<Match>>();
            this.mockGameMatchRepository = new Mock<IRepository<GameMatch>>();
            this.service = new MatchesService(this.mockMatchesRepository.Object, this.mockGameMatchRepository.Object);
        }

        [Fact]
        public async Task ChangeDoublePointsMustChangeMatchDoublePointsFromFalseToTrueOrReverse()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);

            dbContext.GameMatch.Add(new GameMatch { GameId = 1, MatchId = 1, DoublePoints = true });
            dbContext.GameMatch.Add(new GameMatch { GameId = 2, MatchId = 2, DoublePoints = false });
            await dbContext.SaveChangesAsync();

            var matchesRepository = new EfDeletableEntityRepository<Match>(dbContext);
            var gameMatchRepository = new EfRepository<GameMatch>(dbContext);

            var matchesService = new MatchesService(matchesRepository, gameMatchRepository);

            await matchesService.ChangeDoublePoints(1, 1);
            Assert.True(dbContext.GameMatch.FirstOrDefault(gm => gm.GameId == 1 && gm.MatchId == 1).DoublePoints == false);
            await matchesService.ChangeDoublePoints(2, 2);
            Assert.True(dbContext.GameMatch.FirstOrDefault(gm => gm.GameId == 2 && gm.MatchId == 2).DoublePoints == true);
        }

        [Fact]
        public async Task GetAllByGameIdMustReturnAllMatchesByGameId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);

            dbContext.Matches.Add(new Match { Id = 1, StartTime = DateTime.Now.AddDays(1), Games = new List<GameMatch>() });
            dbContext.Matches.Add(new Match { Id = 2, StartTime = DateTime.Now.AddDays(-1), Games = new List<GameMatch>() });
            dbContext.Games.Add(new Game { Id = 1, Matches = new List<GameMatch>() });
            await dbContext.SaveChangesAsync();

            var game = dbContext.Games.FirstOrDefault(g => g.Id == 1);
            game.Matches.Add(new GameMatch { MatchId = 1 });
            await dbContext.SaveChangesAsync();

            var matchesRepository = new EfDeletableEntityRepository<Match>(dbContext);
            var gameMatchRepository = new EfRepository<GameMatch>(dbContext);

            var matchesService = new MatchesService(matchesRepository, gameMatchRepository);
            var result = matchesService.GetAllByGameId<MatchInGameViewModel>(1);
            Assert.True(result.Count() == 0);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("SportBattles.Web.ViewModels"));
    }
}
