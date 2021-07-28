namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services;
    using SportBattles.Services.Mapping;

    public class GamesService : IGamesService
    {
        private readonly IDeletableEntityRepository<GameType> gameTypeRepository;
        private readonly IDeletableEntityRepository<Game> gameRepository;
        private readonly IDeletableEntityRepository<Match> matchRepository;
        private readonly IDeletableEntityRepository<Tournament> tournamentRepository;
        private readonly ITeamService teamService;

        public GamesService(
            IDeletableEntityRepository<GameType> gameTypeRepository,
            IDeletableEntityRepository<Game> gameRepository,
            IDeletableEntityRepository<Match> matchRepository,
            IDeletableEntityRepository<Tournament> tournamentRepository,
            ITeamService teamService)
        {
            this.gameTypeRepository = gameTypeRepository;
            this.gameRepository = gameRepository;
            this.matchRepository = matchRepository;
            this.tournamentRepository = tournamentRepository;
            this.teamService = teamService;
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

        public async Task AddType(string name, string description)
        {
            var gameType = new GameType
            {
                Name = name,
                Description = description,
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

        public IEnumerable<T> GetAllTypes<T>()
        {
            return this.gameTypeRepository.AllAsNoTracking().OrderBy(g => g.Name).To<T>().ToList();
        }

        public async Task AddMatches(int gameId, IEnumerable<FootballMatch> footballMatches)
        {
            var game = this.gameRepository.All().FirstOrDefault(g => g.Id == gameId);

            foreach (var match in footballMatches)
            {
                var tournamentId = this.tournamentRepository.AllAsNoTracking().Where(t => t.Name == match.Tournament).Select(t => t.Id).FirstOrDefault();

                var homeTeamId = await this.teamService.Add(match.HomeTeam, match.HomeTeamEmblemUrl, match.HomeTeamCountry);
                var awayTeamId = await this.teamService.Add(match.AwayTeam, match.AwayTeamEmblemUrl, match.AwayTeamCountry);

                var currMatch = this.matchRepository.AllAsNoTracking().FirstOrDefault(m => m.HomeTeamId == homeTeamId && m.AwayTeamId == awayTeamId && m.StartTime == match.StartTimeUTC);
                if (currMatch == null)
                {
                    currMatch = new Match
                    {
                        StartTime = match.StartTimeUTC,
                        TournamentId = tournamentId,
                        HomeTeamId = homeTeamId,
                        AwayTeamId = awayTeamId,
                    };
                    await this.matchRepository.AddAsync(currMatch);
                    await this.matchRepository.SaveChangesAsync();
                }

                if (game.Matches.Any(m => m.Id == currMatch.Id))
                {
                    continue;
                }

                game.Matches.Add(currMatch);
                await this.gameRepository.SaveChangesAsync();
            }
        }
    }
}
