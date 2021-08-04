namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services;
    using SportBattles.Services.Mapping;
    using SportBattles.Web.ViewModels.Administration.Game;

    public class GamesService : IGamesService
    {
        private readonly IDeletableEntityRepository<GameType> gameTypeRepository;
        private readonly IDeletableEntityRepository<GamePoint> gamePointRepository;
        private readonly IDeletableEntityRepository<Game> gameRepository;
        private readonly IDeletableEntityRepository<Match> matchRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Tournament> tournamentRepository;
        private readonly ITeamsService teamService;

        public GamesService(
            IDeletableEntityRepository<GameType> gameTypeRepository,
            IDeletableEntityRepository<GamePoint> gamePointRepository,
            IDeletableEntityRepository<Game> gameRepository,
            IDeletableEntityRepository<Match> matchRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Tournament> tournamentRepository,
            ITeamsService teamService)
        {
            this.gameTypeRepository = gameTypeRepository;
            this.gamePointRepository = gamePointRepository;
            this.gameRepository = gameRepository;
            this.matchRepository = matchRepository;
            this.userRepository = userRepository;
            this.tournamentRepository = tournamentRepository;
            this.teamService = teamService;
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

        public IEnumerable<T> GetAllStarted<T>()
        {
            return this.gameRepository.AllAsNoTracking().Where(g => g.Started).OrderBy(g => g.Name).To<T>().ToList();
        }

        public IEnumerable<T> GetAllTypes<T>()
        {
            return this.gameTypeRepository.AllAsNoTracking().OrderBy(g => g.Name).To<T>().ToList();
        }

        public IEnumerable<T> GetAllGamePoints<T>()
        {
            return this.gamePointRepository.AllAsNoTracking().OrderBy(gp => gp.Name).To<T>().ToList();
        }

        public async Task AddMatches(int gameId, IEnumerable<FootballMatch> footballMatches)
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
    }
}
