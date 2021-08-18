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
    using SportBattles.Web.ViewModels.Game;
    using SportBattles.Web.ViewModels.Home;
    using Xunit;

    using Match = SportBattles.Data.Models.Match;

    public class GameServiceTest
    {
        private Mock<IDeletableEntityRepository<GameType>> mockGameTypeRepository;
        private Mock<IDeletableEntityRepository<GamePoint>> mockGamePointRepository;
        private Mock<IDeletableEntityRepository<Game>> mockGameRepository;
        private Mock<IDeletableEntityRepository<Match>> mockMatchRepository;
        private Mock<IDeletableEntityRepository<TennisMatch>> mockTennisMatchRepository;
        private Mock<IDeletableEntityRepository<ApplicationUser>> mockUserRepository;
        private Mock<IDeletableEntityRepository<Tournament>> mockTournamentRepository;
        private Mock<IDeletableEntityRepository<Prediction>> mockPredictionRepository;
        private Mock<IDeletableEntityRepository<TennisPrediction>> mockTennisPredictionRepository;
        private Mock<IDeletableEntityRepository<Country>> mockCountryRepository;
        private Mock<IDeletableEntityRepository<Sport>> mockSportRepository;
        private Mock<ITeamsService> mockTeamService;
        private Mock<ITennisPlayersService> mockTennisPlayersService;

        private GamesService service;

        public GameServiceTest()
        {
            this.InitializeMapper();
            this.mockGameTypeRepository = new Mock<IDeletableEntityRepository<GameType>>();
            this.mockGamePointRepository = new Mock<IDeletableEntityRepository<GamePoint>>();
            this.mockGameRepository = new Mock<IDeletableEntityRepository<Game>>();
            this.mockMatchRepository = new Mock<IDeletableEntityRepository<Match>>();
            this.mockTennisMatchRepository = new Mock<IDeletableEntityRepository<TennisMatch>>();
            this.mockUserRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            this.mockTournamentRepository = new Mock<IDeletableEntityRepository<Tournament>>();
            this.mockPredictionRepository = new Mock<IDeletableEntityRepository<Prediction>>();
            this.mockTennisPredictionRepository = new Mock<IDeletableEntityRepository<TennisPrediction>>();
            this.mockCountryRepository = new Mock<IDeletableEntityRepository<Country>>();
            this.mockSportRepository = new Mock<IDeletableEntityRepository<Sport>>();
            this.mockTeamService = new Mock<ITeamsService>();
            this.mockTennisPlayersService = new Mock<ITennisPlayersService>();

            this.service = new GamesService(this.mockGameTypeRepository.Object, this.mockGamePointRepository.Object, this.mockGameRepository.Object, this.mockMatchRepository.Object, this.mockTennisMatchRepository.Object, this.mockUserRepository.Object, this.mockTournamentRepository.Object, this.mockPredictionRepository.Object, this.mockTennisPredictionRepository.Object, this.mockCountryRepository.Object, this.mockSportRepository.Object, this.mockTeamService.Object, this.mockTennisPlayersService.Object);
        }

        [Fact]
        public async Task AddToGameAddedGameSuccesfull()
        {
            var game = new List<Game>();
            this.mockGameRepository.Setup(x => x.AddAsync(It.IsAny<Game>())).Callback((Game g) => game.Add(g));

            await this.service.Add("Test Game", 1);

            Assert.True(game.Count == 1);
            Assert.Contains(game, g => g.Name == "Test Game");
        }

        [Fact]
        public async Task AddTypeAddedTypeSuccesfull()
        {
            var gameType = new List<GameType>();
            this.mockGameTypeRepository.Setup(x => x.AddAsync(It.IsAny<GameType>())).Callback((GameType g) => gameType.Add(g));

            var scoringPointsInputModel = new List<GamePointInputModel>() { new GamePointInputModel { Id = 1, Value = 1 } };

            var scoringPoints = new List<GamePoint>() { new GamePoint { Id = 1, Name = "Test GamePoint" } };
            this.mockGamePointRepository.Setup(x => x.All()).Returns(scoringPoints.AsQueryable());

            await this.service.AddType(new GameTypeInputModel { Name = "Test Game Type", Description = "Test Description", SelectedScoringPoints = scoringPointsInputModel });

            Assert.True(gameType.Count == 1);
            Assert.Contains(gameType, g => g.Name == "Test Game Type");

            scoringPointsInputModel = new List<GamePointInputModel>() { new GamePointInputModel { Id = 2, Value = 1 } };
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.service.AddType(new GameTypeInputModel { Name = "Test Game Type", Description = "Test Description", SelectedScoringPoints = scoringPointsInputModel }));
        }

        [Fact]
        public async Task IsDuplicateMustReturnTrueIfNameAndIdIsAlreadyExist()
        {
            var game = new List<Game>();
            this.mockGameRepository.Setup(x => x.AddAsync(It.IsAny<Game>())).Callback((Game g) => game.Add(g));
            this.mockGameRepository.Setup(x => x.AllAsNoTracking()).Returns(game.AsQueryable());

            await this.service.Add("Test Game", 1);

            Assert.True(this.service.IsDuplicate("Test Game", 1));
            Assert.False(this.service.IsDuplicate("Test Game2", 1));
        }

        [Fact]
        public async Task IsDuplicateTypeNameMustReturnTrueIfNameAlreadyExist()
        {
            var gameType = new List<GameType>();
            this.mockGameTypeRepository.Setup(x => x.AddAsync(It.IsAny<GameType>())).Callback((GameType g) => gameType.Add(g));
            this.mockGameTypeRepository.Setup(x => x.AllAsNoTracking()).Returns(gameType.AsQueryable());

            var scoringPointsInputModel = new List<GamePointInputModel>() { new GamePointInputModel { Id = 1, Value = 1 } };
            var scoringPoints = new List<GamePoint>() { new GamePoint { Id = 1, Name = "Test GamePoint" } };
            this.mockGamePointRepository.Setup(x => x.All()).Returns(scoringPoints.AsQueryable());

            await this.service.AddType(new GameTypeInputModel { Name = "Test Game Type", Description = "Test Description", SelectedScoringPoints = scoringPointsInputModel });

            Assert.True(this.service.IsDuplicateTypeName("Test Game Type"));
            Assert.False(this.service.IsDuplicateTypeName("Test Game Type2"));
        }

        [Fact]
        public async Task JoinMustAddUserToGame()
        {
            var game = new List<Game>() { new Game { Id = 1, Name = "Test Game", Users = new List<ApplicationUser>() } };
            var user = new List<ApplicationUser> { new ApplicationUser { Id = "Test Id", UserName = "Test User" } };
            this.mockGameRepository.Setup(x => x.All()).Returns(game.AsQueryable());
            this.mockUserRepository.Setup(x => x.All()).Returns(user.AsQueryable());

            await this.service.Join(1, "Test Id");

            Assert.Contains(game.Select(g => g.Users).ToList(), u => u.Any(u => u.Id == "Test Id"));
            Assert.DoesNotContain(game.Select(g => g.Users).ToList(), u => u.Any(u => u.Id == "Test Id2"));

            await this.service.Join(2, "Test Id");
        }

        [Fact]
        public async Task JoinMustNotAddUserToGameIfUserOrGameNotFound()
        {
            var game = new List<Game>() { new Game { Id = 1, Name = "Test Game", Users = new List<ApplicationUser>() } };
            var user = new List<ApplicationUser> { new ApplicationUser { Id = "Test Id", UserName = "Test User" } };
            this.mockGameRepository.Setup(x => x.All()).Returns(game.AsQueryable());
            this.mockUserRepository.Setup(x => x.All()).Returns(user.AsQueryable());

            await this.service.Join(1, "Test Id2");
            Assert.Empty(game.FirstOrDefault(g => g.Id == 1).Users.ToList());
        }

        [Fact]
        public async Task DeleteGameMustMarkGameAsDeleted()
        {
            var game = new List<Game>()
            {
                new Game { Id = 1, Name = "Test Game", Users = new List<ApplicationUser>() },
                new Game { Id = 2, Name = "Test Game2", Users = new List<ApplicationUser>() },
            };
            this.mockGameRepository.Setup(x => x.All()).Returns(game.AsQueryable());
            this.mockGameRepository.Setup(x => x.Delete(It.IsAny<Game>())).Callback((Game g) => game.Remove(g));

            await this.service.Delete(1);
            Assert.True(game.Count == 1);
        }

        [Fact]
        public async Task JoinMustNotAddUserIfUserIsAlreadyInGame()
        {
            var game = new List<Game>() { new Game { Id = 1, Name = "Test Game", Users = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id", UserName = "Test User" } } } };
            var user = new List<ApplicationUser> { new ApplicationUser { Id = "Test Id", UserName = "Test User" } };
            this.mockGameRepository.Setup(x => x.All()).Returns(game.AsQueryable());
            this.mockUserRepository.Setup(x => x.All()).Returns(user.AsQueryable());

            await this.service.Join(1, "Test Id");
            Assert.True(game.FirstOrDefault(g => g.Id == 1).Users.ToList().Count == 1);
        }

        [Fact]
        public async Task ChangeStatusMustChangeStatusOfTheGameFromTrueToFalseAndReverse()
        {
            var game = new List<Game>()
            {
                new Game { Id = 1, Name = "Test Game", Started = true },
                new Game { Id = 2, Name = "Test Game2", Started = false },
            };
            this.mockGameRepository.Setup(x => x.All()).Returns(game.AsQueryable());

            await this.service.ChangeStatus(1);
            Assert.True(game.FirstOrDefault(g => g.Id == 1).Started == false);

            await this.service.ChangeStatus(2);
            Assert.True(game.FirstOrDefault(g => g.Id == 2).Started == true);
        }

        [Fact]
        public void GetAllMustReturnAllGames()
        {
            var game = new List<Game>()
            {
                new Game { Id = 1, Name = "Test Game", Started = true },
                new Game { Id = 2, Name = "Test Game2", Started = false },
                new Game { Id = 3, Name = "Test Game3", Started = false },
            };
            this.mockGameRepository.Setup(x => x.All()).Returns(game.AsQueryable());

            var result = this.service.GetAll<SportBattles.Web.ViewModels.Administration.Game.GameViewModel>();
            Assert.True(game.Count == 3);
        }

        [Fact]
        public void GetUserGamesMustReturnAllGamesByUserId()
        {
            var user = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "Test Id", UserName = "Test User" },
                new ApplicationUser { Id = "Test Id2", UserName = "Test User2" },
            };
            var gameType = new GameType { Name = "Test GameType" };
            var game = new List<Game>()
            {
                new Game { Id = 1, Name = "Test Game", Started = true, GameType = gameType, Users = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id", UserName = "Test User" }, { new ApplicationUser { Id = "Test Id2", UserName = "Test User2" } } } },
                new Game { Id = 2, Name = "Test Game2", Started = true, GameType = gameType, Users = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id", UserName = "Test User" } } },
                new Game { Id = 3, Name = "Test Game3", Started = true, GameType = gameType, Users = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id2", UserName = "Test User2" } } },
            };
            this.mockGameRepository.Setup(x => x.AllAsNoTracking()).Returns(game.AsQueryable());
            this.mockUserRepository.Setup(x => x.AllAsNoTracking()).Returns(user.AsQueryable());

            var result = this.service.GetUserGames<SportBattles.Web.ViewModels.Administration.Game.GameViewModel>("Test Id");
            Assert.True(result.Count() == 2);
        }

        [Fact]
        public void GetAllStartedGamesMustReturnAllGamesByUserIdInWhichUserInNotJoinYetOrAllStartedGamesIfUserIdIsNull()
        {
            var user = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "Test Id", UserName = "Test User" },
                new ApplicationUser { Id = "Test Id2", UserName = "Test User2" },
            };
            var gameType = new GameType { Name = "Test GameType" };
            var game = new List<Game>()
            {
                new Game { Id = 1, Name = "Test Game", Started = true, GameType = gameType, Users = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id", UserName = "Test User" }, { new ApplicationUser { Id = "Test Id2", UserName = "Test User2" } } } },
                new Game { Id = 2, Name = "Test Game2", Started = true, GameType = gameType, Users = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id", UserName = "Test User" } } },
                new Game { Id = 3, Name = "Test Game3", Started = true, GameType = gameType, Users = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id2", UserName = "Test User2" } } },
            };
            this.mockGameRepository.Setup(x => x.AllAsNoTracking()).Returns(game.AsQueryable());
            this.mockUserRepository.Setup(x => x.AllAsNoTracking()).Returns(user.AsQueryable());

            var result = this.service.GetAllStarted<SportBattles.Web.ViewModels.Administration.Game.GameViewModel>("Test Id");
            Assert.True(result.Count() == 1);
            Assert.Contains(result, g => g.Id == 3);

            result = this.service.GetAllStarted<SportBattles.Web.ViewModels.Administration.Game.GameViewModel>(null);
            Assert.True(result.Count() == 3);
        }

        [Fact]
        public void GetAllTypesMustReturnAllGameTypes()
        {
            var gameTypes = new List<GameType>()
            {
                new GameType { Id = 1, Name = "Test Game", Description = "Test Description" },
                new GameType { Id = 2, Name = "Test Game2", Description = "Test Description2" },
                new GameType { Id = 3, Name = "Test Game3", Description = "Test Description3" },
            };
            this.mockGameTypeRepository.Setup(x => x.AllAsNoTracking()).Returns(gameTypes.AsQueryable());

            var result = this.service.GetAllTypes<GameTypeViewModel>();
            Assert.True(result.Count() == 3);
        }

        [Fact]
        public void GetTopGamesMustReturnCountGamesWhichIsStarted()
        {
            var usersCollectionOne = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id", UserName = "Test User" }, { new ApplicationUser { Id = "Test Id2", UserName = "Test User2" } } };
            var usersCollectionTwo = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id", UserName = "Test User" }, { new ApplicationUser { Id = "Test Id2", UserName = "Test User2" } }, { new ApplicationUser { Id = "Test Id3", UserName = "Test User3" } } };
            var matchesCollectionOne = new List<GameTennisMatch>() { new GameTennisMatch { TennisMatchId = 1 }, new GameTennisMatch { TennisMatchId = 2 } };
            var matchesCollectionTwo = new List<GameMatch>() { new GameMatch { MatchId = 1 } };
            var gameType = new GameType { Name = "Test GameType" };
            var game = new List<Game>()
            {
                new Game { Id = 1, Name = "Test Game", Started = true, GameType = gameType, Users = usersCollectionOne },
                new Game { Id = 2, Name = "Test Game2", Started = true, GameType = gameType, Users = usersCollectionTwo, Matches = matchesCollectionTwo },
                new Game { Id = 3, Name = "Test Game3", Started = true, GameType = gameType, Users = usersCollectionTwo, TennisMatches = matchesCollectionOne },
            };
            this.mockGameRepository.Setup(x => x.AllAsNoTracking()).Returns(game.AsQueryable().OrderByDescending(g => g.Users.Count).ThenByDescending(g => g.Matches.Count + g.TennisMatches.Count));

            var result = this.service.GetTopGames<TopGamesComponentViewModel>(2);
            Assert.True(result.Count() == 2);
            Assert.True(result.First().Id == 3);
        }

        [Fact]
        public void GetLatestGamesMustReturnGamesOrderByTimeOfCreation()
        {
            var gameTypeFootball = new GameType { Name = "Football" };
            var gameTypeTennis = new GameType { Name = "Tennis" };
            var game = new List<Game>()
            {
                new Game { Id = 1, Name = "Test Game", Started = true, GameType = gameTypeFootball, CreatedOn = DateTime.Today.AddDays(-1) },
                new Game { Id = 2, Name = "Test Game2", Started = true, GameType = gameTypeTennis, CreatedOn = DateTime.Today.AddHours(-1) },
                new Game { Id = 3, Name = "Test Game3", Started = true, GameType = gameTypeTennis, CreatedOn = DateTime.Today },
                new Game { Id = 4, Name = "Test Game4", Started = true, GameType = gameTypeFootball, CreatedOn = DateTime.Today },
            };
            this.mockGameRepository.Setup(x => x.AllAsNoTracking()).Returns(game.AsQueryable().OrderByDescending(g => g.CreatedOn));

            var result = this.service.GetLatest<LatestGamesViewModel>();
            Assert.True(result.Count() == 2);
            Assert.True(result.First().Id == 4);
            Assert.True(result.Last().Id == 3);
        }

        [Fact]
        public void GetStartedGameById()
        {
            var gameTypeFootball = new GameType { Name = "Football" };
            var gameTypeTennis = new GameType { Name = "Tennis" };
            var game = new List<Game>()
            {
                new Game { Id = 1, Name = "Test Game", Started = true, GameType = gameTypeFootball, CreatedOn = DateTime.Today.AddDays(-1) },
                new Game { Id = 2, Name = "Test Game2", Started = true, GameType = gameTypeTennis, CreatedOn = DateTime.Today.AddHours(-1) },
                new Game { Id = 3, Name = "Test Game3", Started = false, GameType = gameTypeTennis, CreatedOn = DateTime.Today },
                new Game { Id = 4, Name = "Test Game4", Started = true, GameType = gameTypeFootball, CreatedOn = DateTime.Today },
            };
            this.mockGameRepository.Setup(x => x.AllAsNoTracking()).Returns(game.AsQueryable());

            var result = this.service.GetGame<GameInfoViewModel>(1);
            Assert.True(result.Name == "Test Game");
        }

        [Fact]
        public void IsUserInGameMustReturnTrueIfUserIdIsInGame()
        {
            var user = new ApplicationUser { Id = "Test Id", UserName = "Test User" };
            var game = new List<Game>() { new Game { Id = 1, Name = "Test Game", Users = new List<ApplicationUser>() { user } } };
            this.mockGameRepository.Setup(x => x.AllAsNoTracking()).Returns(game.AsQueryable());

            Assert.True(this.service.IsUserInGame("Test Id", 1));
        }

        [Fact]
        public void RankingMustReturnRankingViewModelOfAllUsersInGame()
        {
            var tennisPredictions = new List<TennisPrediction> { new TennisPrediction { UserId = "Test Id", GameId = 2, Points = 1 }, new TennisPrediction { UserId = "Test Id2", GameId = 2, Points = 5 }, new TennisPrediction { UserId = "Test Id3", GameId = 2, Points = 3 } };
            var footballPredictions = new List<Prediction> { new Prediction { UserId = "Test Id", GameId = 3, Points = 1 }, new Prediction { UserId = "Test Id2", GameId = 3, Points = 5 }, new Prediction { UserId = "Test Id3", GameId = 3, Points = 3 } };
            var usersCollectionOne = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id", UserName = "Test User" }, { new ApplicationUser { Id = "Test Id2", UserName = "Test User2" } } };
            var usersCollectionTwo = new List<ApplicationUser>() { new ApplicationUser { Id = "Test Id", UserName = "Test User", Games = new List<Game>() { new Game { Id = 2 }, new Game { Id = 3 } }, TennisPredictions = tennisPredictions }, { new ApplicationUser { Id = "Test Id2", UserName = "Test User2", Games = new List<Game>() { new Game { Id = 2 }, new Game { Id = 3 } }, TennisPredictions = tennisPredictions } }, { new ApplicationUser { Id = "Test Id3", UserName = "Test User3", Games = new List<Game>() { new Game { Id = 2 }, new Game { Id = 3 } }, Predictions = footballPredictions } } };
            var matchesCollectionOne = new List<GameTennisMatch>() { new GameTennisMatch { TennisMatchId = 1 }, new GameTennisMatch { TennisMatchId = 2 } };
            var matchesCollectionTwo = new List<GameMatch>() { new GameMatch { MatchId = 1 } };
            var gameTypes = new List<GameType> { new GameType { Name = "FootballType" }, new GameType { Name = "TennisType" } };
            var games = new List<Game>()
            {
                new Game { Id = 1, Name = "Test Game", Started = true, GameType = gameTypes[0], Users = usersCollectionOne },
                new Game { Id = 2, Name = "Test Game2", Started = true, GameType = gameTypes[1], Users = usersCollectionTwo, Matches = matchesCollectionTwo, TennisPredictions = tennisPredictions },
                new Game { Id = 3, Name = "Test Game3", Started = true, GameType = gameTypes[0], Users = usersCollectionTwo, Matches = matchesCollectionTwo, Predictions = footballPredictions },
            };
            this.mockUserRepository.Setup(x => x.AllAsNoTracking()).Returns(usersCollectionTwo.AsQueryable());
            this.mockGameTypeRepository.Setup(x => x.AllAsNoTracking()).Returns(gameTypes.AsQueryable());
            this.mockPredictionRepository.Setup(x => x.AllAsNoTracking()).Returns(footballPredictions.AsQueryable());
            this.mockTennisPredictionRepository.Setup(x => x.AllAsNoTracking()).Returns(tennisPredictions.AsQueryable());
            this.mockGameRepository.Setup(x => x.AllAsNoTracking()).Returns(games.AsQueryable());

            var result = this.service.GetRanking(2);
            Assert.True(result.Count() == 3);
            Assert.True(result.First().UserName == "Test User2");

            result = this.service.GetRanking(3);
            Assert.True(result.Count() == 3);
            Assert.True(result.First().UserName == "Test User2");
        }

        [Fact]
        public async Task AddMatchesToGameAddedMatchSuccesfullWithGivenFootballMatchServiceModel()
        {
            var countryBG = new Country { Name = "Bulgaria" };
            var countryES = new Country { Name = "Spain" };
            var teams = new List<Team>()
            {
                new Team { Name = "Test Team 1", Id = 1, Country = countryBG, EmblemUrl = "emblem1" },
                new Team { Name = "Test Team 2", Id = 2, Country = countryES, EmblemUrl = "emblem2" },
                new Team { Name = "Barcelona", Id = 3, Country = countryES, EmblemUrl = "barca.png" },
                new Team { Name = "Real Madrid", Id = 4, Country = countryES, EmblemUrl = "real.png" },
            };
            var tournaments = new List<Tournament>()
            {
                new Tournament { Id = 1, Country = countryBG, Name = "Test Tournament 1" },
                new Tournament { Id = 2, Country = countryES, Name = "Test Tournament 2" },
            };

            this.mockTournamentRepository.Setup(x => x.AllAsNoTracking()).Returns(tournaments.AsQueryable());
            var matches = new List<Match>()
            {
                new Match { Id = 1, StartTime = DateTime.Today.AddHours(2), Status = "NS", HomeTeam = teams[0], AwayTeam = teams[1], Tournament = tournaments[0] },
                new Match { Id = 2, StartTime = DateTime.Today.AddHours(2), Status = "NS", HomeTeam = teams[2], AwayTeam = teams[3], Tournament = tournaments[1] },
            };
            var gameMatches = new List<GameMatch>()
            {
                new GameMatch { Match = matches[0] },
                new GameMatch { Match = matches[1] },
            };
            var games = new List<Game>() { new Game { Id = 1, Matches = gameMatches, } };
            this.mockGameRepository.Setup(x => x.All()).Returns(games.AsQueryable());

            var matchesServiceModel = new List<FootballMatchServiceModel>
            {
                new FootballMatchServiceModel
                {
                    Tournament = "Test Tournament 2", Country = countryES.Name, HomeTeam = "Barcelona", AwayTeam = "Real Madrid", HomeTeamEmblemUrl = "barca.png", AwayTeamEmblemUrl = "real.png", HomeTeamCountry = "Spain", AwayTeamCountry = "Spain",
                },
            };
            this.mockMatchRepository.Setup(x => x.AllAsNoTracking()).Returns(matches.AsQueryable());
            this.mockMatchRepository.Setup(x => x.AddAsync(It.IsAny<Match>())).Callback((Match m) => matches.Add(m));

            await this.service.AddMatches(1, matchesServiceModel);
            Assert.Contains(matches, m => m.HomeTeam.Name == "Barcelona");

            matchesServiceModel = new List<FootballMatchServiceModel>
            {
                new FootballMatchServiceModel
                {
                    Tournament = "Test Tournament 2", Country = countryES.Name, HomeTeam = "Barcelona", AwayTeam = "Real Madrid", HomeTeamEmblemUrl = "real.png", AwayTeamEmblemUrl = "barca.png", HomeTeamCountry = "Spain", AwayTeamCountry = "Spain",
                    StartTimeUTC = DateTime.Today.AddHours(2),
                },
            };
            matches.Add(new Match { Id = 3, StartTime = DateTime.Today.AddHours(2), Status = "NS", HomeTeamId = 3, AwayTeamId = 4, Tournament = tournaments[1] });

            var mockTeamRepository = new Mock<IDeletableEntityRepository<Team>>();
            var mockedService = new TeamsService(mockTeamRepository.Object, this.mockCountryRepository.Object);
            mockTeamRepository.Setup(x => x.All()).Returns(teams.AsQueryable());
            this.mockTeamService.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(async (string a, string b, string c) => await mockedService.Add(a, b, c));

            this.mockGameRepository.Setup(x => x.AddAsync(It.IsAny<Game>())).Callback((Game g) => games.Add(g));
            await this.service.AddMatches(1, matchesServiceModel);
            Assert.Contains(matches, m => m.HomeTeamId == 3 && m.AwayTeamId == 4);
        }

        [Fact]
        public async Task AddTennisMatchesToGameAddedTennisMatchSuccesfullWithGivenTennisMatchServiceModel()
        {
            var countryBG = new Country { Name = "Bulgaria" };
            var countryES = new Country { Name = "Spain" };
            var players = new List<TennisPlayer>()
            {
                new TennisPlayer { Name = "Test Team 1", Id = 1, Country = countryBG, PictureUrl = "pic1" },
                new TennisPlayer { Name = "Test Team 2", Id = 2, Country = countryES, PictureUrl = "pic2" },
                new TennisPlayer { Name = "Grigor Dimitrov", Id = 3, Country = countryES, PictureUrl = "grisho.png" },
                new TennisPlayer { Name = "Rafael Nadal", Id = 4, Country = countryES, PictureUrl = "nadal.png" },
            };
            var tournaments = new List<Tournament>()
            {
                new Tournament { Id = 1, Country = countryBG, Name = "Test Tournament 1" },
                new Tournament { Id = 2, Country = countryES, Name = "Test Tournament 2" },
            };

            this.mockTournamentRepository.Setup(x => x.AllAsNoTracking()).Returns(tournaments.AsQueryable());
            this.mockTournamentRepository.Setup(x => x.AddAsync(It.IsAny<Tournament>())).Callback((Tournament t) => tournaments.Add(t));
            var tennisMatches = new List<TennisMatch>()
            {
                new TennisMatch { Id = 1, StartTime = DateTime.Today.AddHours(2), Status = "NS", HomePlayer = players[0], AwayPlayer = players[1], Tournament = tournaments[0] },
                new TennisMatch { Id = 2, StartTime = DateTime.Today.AddHours(2), Status = "NS", HomePlayer = players[2], AwayPlayer = players[3], Tournament = tournaments[1] },
            };
            var gameTennisMatches = new List<GameTennisMatch>()
            {
                new GameTennisMatch { TennisMatch = tennisMatches[0] },
                new GameTennisMatch { TennisMatch = tennisMatches[1] },
            };
            var games = new List<Game>() { new Game { Id = 1, TennisMatches = gameTennisMatches, } };
            this.mockGameRepository.Setup(x => x.All()).Returns(games.AsQueryable());

            var tennisMatchesServiceModel = new List<TennisMatchServiceModel>
            {
                new TennisMatchServiceModel
                {
                    Tournament = "Test Tournament 2", Country = countryES.Name, HomeTeam = "Grigor Dimitrov", AwayTeam = "Rafael Nadal", HomeTeamCountry = "Spain", AwayTeamCountry = "Spain",
                },
            };
            this.mockTennisMatchRepository.Setup(x => x.AllAsNoTracking()).Returns(tennisMatches.AsQueryable());
            this.mockTennisMatchRepository.Setup(x => x.AddAsync(It.IsAny<TennisMatch>())).Callback((TennisMatch m) => tennisMatches.Add(m));

            await this.service.AddTennisMatches(1, tennisMatchesServiceModel);
            Assert.Contains(tennisMatches, m => m.HomePlayer.Name == "Grigor Dimitrov");

            tennisMatchesServiceModel = new List<TennisMatchServiceModel>
            {
                new TennisMatchServiceModel
                {
                    Tournament = "Test Tournament 2", Country = countryES.Name, HomeTeam = "Grigor Dimitrov", AwayTeam = "Rafael Nadal", HomeTeamCountry = "Spain", AwayTeamCountry = "Spain",
                    StartTimeUTC = DateTime.Today.AddHours(2),
                },
            };
            tennisMatches.Add(new TennisMatch { Id = 3, StartTime = DateTime.Today.AddHours(2), Status = "NS", HomePlayerId = 3, AwayPlayerId = 4, Tournament = tournaments[1] });

            var mockPlayerRepository = new Mock<IDeletableEntityRepository<TennisPlayer>>();
            var mockedService = new TennisPlayersService(mockPlayerRepository.Object, this.mockCountryRepository.Object, new Mock<ITennisExplorerScraperService>().Object);
            mockPlayerRepository.Setup(x => x.All()).Returns(players.AsQueryable());
            this.mockTennisPlayersService.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>())).Returns(async (string a, string b) => await mockedService.Add(a, b));

            this.mockGameRepository.Setup(x => x.AddAsync(It.IsAny<Game>())).Callback((Game g) => games.Add(g));
            await this.service.AddTennisMatches(1, tennisMatchesServiceModel);
            Assert.Contains(tennisMatches, m => m.HomePlayerId == 3 && m.AwayPlayerId == 4);

            tennisMatchesServiceModel = new List<TennisMatchServiceModel>
            {
                new TennisMatchServiceModel
                {
                    Tournament = "Test Tournament 3", Country = countryES.Name, HomeTeam = "Grigor Dimitrov", AwayTeam = "Rafael Nadal", HomeTeamCountry = "Spain", AwayTeamCountry = "Spain",
                    StartTimeUTC = DateTime.Today.AddHours(2),
                },
            };
            var sports = new List<Sport>() { new Sport { Id = 1, Name = "Tennis" } };
            this.mockSportRepository.Setup(x => x.AllAsNoTracking()).Returns(sports.AsQueryable());
            var countries = new List<Country>() { countryBG, countryES };
            this.mockCountryRepository.Setup(x => x.AllAsNoTracking()).Returns(countries.AsQueryable());
            await this.service.AddTennisMatches(1, tennisMatchesServiceModel);

            Assert.Contains(tournaments, t => t.Name == "Test Tournament 3");

            tennisMatchesServiceModel = new List<TennisMatchServiceModel>
            {
                new TennisMatchServiceModel
                {
                    Tournament = "Test Tournament 2", Country = countryES.Name, HomeTeam = "Grigor Dimitrov", AwayTeam = "Rafael Nadal", HomeTeamCountry = "Spain", AwayTeamCountry = "Spain",
                    StartTimeUTC = DateTime.Today.AddHours(3),
                },
            };
            await this.service.AddTennisMatches(1, tennisMatchesServiceModel);

        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("SportBattles.Web.ViewModels"));
    }
}
