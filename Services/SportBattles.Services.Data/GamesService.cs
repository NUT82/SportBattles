namespace SportBattles.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services;
    using SportBattles.Services.Mapping;
    using SportBattles.Web.ViewModels.Administration.Game;
    using SportBattles.Web.ViewModels.Game;

    public class GamesService : IGamesService
    {
        private readonly IDeletableEntityRepository<GameType> gameTypeRepository;
        private readonly IDeletableEntityRepository<GamePoint> gamePointRepository;
        private readonly IDeletableEntityRepository<Game> gameRepository;
        private readonly IDeletableEntityRepository<Match> matchRepository;
        private readonly IDeletableEntityRepository<TennisMatch> tennisMatchRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Tournament> tournamentRepository;
        private readonly IDeletableEntityRepository<Prediction> predictionRepository;
        private readonly IDeletableEntityRepository<TennisPrediction> tennisPredictionRepository;
        private readonly IDeletableEntityRepository<Country> countryRepository;
        private readonly IDeletableEntityRepository<Sport> sportRepository;
        private readonly ITeamsService teamService;
        private readonly ITennisPlayersService tennisPlayersService;

        public GamesService(
            IDeletableEntityRepository<GameType> gameTypeRepository,
            IDeletableEntityRepository<GamePoint> gamePointRepository,
            IDeletableEntityRepository<Game> gameRepository,
            IDeletableEntityRepository<Match> matchRepository,
            IDeletableEntityRepository<TennisMatch> tennisMatchRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Tournament> tournamentRepository,
            IDeletableEntityRepository<Prediction> predictionRepository,
            IDeletableEntityRepository<TennisPrediction> tennisPredictionRepository,
            IDeletableEntityRepository<Country> countryRepository,
            IDeletableEntityRepository<Sport> sportRepository,
            ITeamsService teamService,
            ITennisPlayersService tennisPlayersService)
        {
            this.gameTypeRepository = gameTypeRepository;
            this.gamePointRepository = gamePointRepository;
            this.gameRepository = gameRepository;
            this.matchRepository = matchRepository;
            this.tennisMatchRepository = tennisMatchRepository;
            this.userRepository = userRepository;
            this.tournamentRepository = tournamentRepository;
            this.predictionRepository = predictionRepository;
            this.tennisPredictionRepository = tennisPredictionRepository;
            this.countryRepository = countryRepository;
            this.sportRepository = sportRepository;
            this.teamService = teamService;
            this.tennisPlayersService = tennisPlayersService;
        }

        public IEnumerable<RankingViewModel> GetRanking(int gameId)
        {
            var result = new List<RankingViewModel>();
            var users = this.userRepository.AllAsNoTracking().Where(u => u.Games.Any(g => g.Id == gameId)).ToList();
            foreach (var user in users)
            {
                var gameType = this.gameRepository.AllAsNoTracking().Where(g => g.Id == gameId).Select(g => g.GameType.Name).FirstOrDefault();
                var points = 0;
                if (gameType.Contains("Football"))
                {
                    points = this.predictionRepository.AllAsNoTracking().Where(p => p.UserId == user.Id && p.GameId == gameId && p.Points.HasValue).Select(p => (int)p.Points.Value).Sum();
                }
                else if (gameType.Contains("Tennis"))
                {
                    points = this.tennisPredictionRepository.AllAsNoTracking().Where(p => p.UserId == user.Id && p.GameId == gameId && p.Points.HasValue).Select(p => (int)p.Points.Value).Sum();
                }

                result.Add(new RankingViewModel
                {
                    UserName = user.UserName,
                    ProfilePictureId = user.ProfilePictureId,
                    Points = points,
                });
            }

            return result.OrderByDescending(r => r.Points);
        }

        public async Task Join(int gameId, string userId)
        {
            var game = this.gameRepository.All().FirstOrDefault(g => g.Id == gameId);
            var user = this.userRepository.All().FirstOrDefault(u => u.Id == userId);
            if (game == null || user == null)
            {
                return;
            }

            if (game.Users.Any(u => u.Id == userId))
            {
                return;
            }

            game.Users.Add(user);
            await this.gameRepository.SaveChangesAsync();
        }

        public async Task Add(string name, int typeId)
        {
            var game = new Game
            {
                Name = name,
                GameTypeId = typeId,
            };

            await this.gameRepository.AddAsync(game);
            await this.gameRepository.SaveChangesAsync();
        }

        public async Task Delete(int gameId)
        {
            var game = this.gameRepository.All().FirstOrDefault(g => g.Id == gameId);

            this.gameRepository.Delete(game);
            await this.gameRepository.SaveChangesAsync();
        }

        public async Task ChangeStatus(int gameId)
        {
            var game = this.gameRepository.All().FirstOrDefault(g => g.Id == gameId);

            game.Started = !game.Started;
            await this.gameRepository.SaveChangesAsync();
        }

        public async Task AddType(GameTypeInputModel input)
        {
            var gameType = new GameType
            {
                Name = input.Name,
                Description = input.Description,
            };

            foreach (var scoringPoint in input.SelectedScoringPoints)
            {
                var gamePoint = this.gamePointRepository.All().FirstOrDefault(gp => gp.Id == scoringPoint.Id);
                if (gamePoint == null)
                {
                    throw new ArgumentNullException("No GamePoint with this Id");
                }

                gameType.GamePoints.Add(new GamePointGameType { GamePoint = gamePoint, Value = scoringPoint.Value });
            }

            await this.gameTypeRepository.AddAsync(gameType);
            await this.gameTypeRepository.SaveChangesAsync();
        }

        public bool IsDuplicate(string name, int typeId)
        {
            return this.gameRepository.AllAsNoTracking().Any(g => g.Name == name && g.GameTypeId == typeId);
        }

        public bool IsDuplicateTypeName(string name)
        {
            return this.gameTypeRepository.AllAsNoTracking().Any(g => g.Name == name);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.gameRepository.AllAsNoTracking().OrderBy(g => g.Name).To<T>().ToList();
        }

        public IEnumerable<T> GetUserGames<T>(string userId)
        {
            return this.gameRepository.AllAsNoTracking().Where(g => g.Started && g.Users.Any(u => u.Id == userId)).OrderBy(g => g.Name).To<T>().ToList();
        }

        public IDictionary<int, int> UnpredictedMatchesInGameByUserCount(string userId)
        {
            var matches = this.gameRepository.AllAsNoTracking().Where(g => g.Started && g.Users.Any(u => u.Id == userId) && g.GameType.Name.Contains("Football")).Select(g => new KeyValuePair<int, int>(g.Id, g.Matches.Where(m => m.Match.StartTime > DateTime.UtcNow).Count() - g.Predictions.Where(p => p.Match.StartTime > DateTime.UtcNow && p.UserId == userId).Count())).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var tennisMatches = this.gameRepository.AllAsNoTracking().Where(g => g.Started && g.Users.Any(u => u.Id == userId) && g.GameType.Name.Contains("Tennis")).Select(g => new KeyValuePair<int, int>(g.Id, g.TennisMatches.Where(m => m.TennisMatch.StartTime > DateTime.UtcNow).Count() - g.TennisPredictions.Where(p => p.TennisMatch.StartTime > DateTime.UtcNow && p.UserId == userId).Count())).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return matches.Concat(tennisMatches).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public IEnumerable<T> GetAllStarted<T>(string userId)
        {
            if (userId is null)
            {
                return this.gameRepository.AllAsNoTracking().Where(g => g.Started).OrderBy(g => g.Name).To<T>().ToList();
            }

            return this.gameRepository.AllAsNoTracking().Where(g => g.Started && !g.Users.Any(u => u.Id == userId)).OrderBy(g => g.Name).To<T>().ToList();
        }

        public IEnumerable<T> GetAllTypes<T>()
        {
            return this.gameTypeRepository.AllAsNoTracking().OrderBy(g => g.Name).To<T>().ToList();
        }

        public async Task AddMatches(int gameId, IEnumerable<FootballMatchServiceModel> footballMatches)
        {
            var game = this.gameRepository.All().FirstOrDefault(g => g.Id == gameId);

            foreach (var match in footballMatches)
            {
                var tournamentId = this.tournamentRepository.AllAsNoTracking().Where(t => t.Name == match.Tournament && t.Country.Name == match.Country).Select(t => t.Id).FirstOrDefault();

                var homeTeamId = await this.teamService.Add(match.HomeTeam, match.HomeTeamEmblemUrl, match.HomeTeamCountry);
                var awayTeamId = await this.teamService.Add(match.AwayTeam, match.AwayTeamEmblemUrl, match.AwayTeamCountry);

                var currMatch = this.matchRepository.AllAsNoTracking().FirstOrDefault(m => m.HomeTeamId == homeTeamId && m.AwayTeamId == awayTeamId && m.StartTime == match.StartTimeUTC);
                if (currMatch == null)
                {
                    currMatch = new Match
                    {
                        StartTime = match.StartTimeUTC,
                        Status = match.Status,
                        TournamentId = tournamentId,
                        HomeTeamId = homeTeamId,
                        AwayTeamId = awayTeamId,
                    };
                    await this.matchRepository.AddAsync(currMatch);
                    await this.matchRepository.SaveChangesAsync();
                }

                if (game.Matches.Any(m => m.MatchId == currMatch.Id))
                {
                    continue;
                }

                game.Matches.Add(new GameMatch { Match = currMatch });
                await this.gameRepository.SaveChangesAsync();
            }
        }

        public async Task AddTennisMatches(int gameId, IEnumerable<TennisMatchServiceModel> tennisMatches)
        {
            var game = this.gameRepository.All().FirstOrDefault(g => g.Id == gameId);

            foreach (var match in tennisMatches)
            {
                var tournament = this.tournamentRepository.AllAsNoTracking().Where(t => t.Name == match.Tournament && t.Country.Name == match.Country).FirstOrDefault();

                if (tournament == null)
                {
                    tournament = new Tournament
                    {
                        Name = match.Tournament,
                        CountryId = this.countryRepository.AllAsNoTracking().FirstOrDefault(c => c.Name == match.Country).Id,
                        SportId = this.sportRepository.AllAsNoTracking().FirstOrDefault(s => s.Name == "Tennis").Id,
                    };

                    await this.tournamentRepository.AddAsync(tournament);
                    await this.tournamentRepository.SaveChangesAsync();
                }

                var homePlayerId = await this.tennisPlayersService.Add(match.HomeTeam, match.HomeTeamCountry);
                var awayPlayerId = await this.tennisPlayersService.Add(match.AwayTeam, match.AwayTeamCountry);

                var currMatch = this.tennisMatchRepository.All().FirstOrDefault(m => m.HomePlayerId == homePlayerId && m.AwayPlayerId == awayPlayerId && m.Status != "FT");
                if (currMatch == null)
                {
                    currMatch = new TennisMatch
                    {
                        StartTime = match.StartTimeUTC,
                        Status = match.Status,
                        TournamentId = tournament.Id,
                        HomePlayerId = homePlayerId,
                        AwayPlayerId = awayPlayerId,
                    };
                    await this.tennisMatchRepository.AddAsync(currMatch);
                    await this.tennisMatchRepository.SaveChangesAsync();
                }

                if (game.TennisMatches.Any(m => m.TennisMatchId == currMatch.Id))
                {
                    if (match.StartTimeUTC != currMatch.StartTime)
                    {
                        currMatch.StartTime = match.StartTimeUTC;
                        await this.tennisMatchRepository.SaveChangesAsync();
                    }

                    continue;
                }

                game.TennisMatches.Add(new GameTennisMatch { TennisMatch = currMatch });
                await this.gameRepository.SaveChangesAsync();
            }
        }

        public IEnumerable<T> GetTopGames<T>(int count)
        {
            return this.gameRepository.AllAsNoTracking().Where(g => g.Started).OrderByDescending(g => g.Users.Count).ThenByDescending(g => g.Matches.Count + g.TennisMatches.Count).Take(count).To<T>().ToList();
        }

        public IEnumerable<T> GetLatest<T>()
        {
            var result = new List<T>();
            var latestFootballGame = this.gameRepository.AllAsNoTracking().Where(g => g.Started && g.GameType.Name.Contains("Football")).OrderByDescending(g => g.CreatedOn).To<T>().FirstOrDefault();
            result.Add(latestFootballGame);

            var latestTennisGame = this.gameRepository.AllAsNoTracking().Where(g => g.Started && g.GameType.Name.Contains("Tennis")).OrderByDescending(g => g.CreatedOn).To<T>().FirstOrDefault();
            result.Add(latestTennisGame);

            return result;
        }

        public T GetGame<T>(int id)
        {
            return this.gameRepository.AllAsNoTracking().Where(g => g.Started && g.Id == id).To<T>().FirstOrDefault();
        }

        public bool IsUserInGame(string userId, int gameId)
        {
            return this.gameRepository.AllAsNoTracking().Any(g => g.Id == gameId && g.Users.Any(u => u.Id == userId));
        }
    }
}
