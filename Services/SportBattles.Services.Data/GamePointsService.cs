namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class GamePointsService : IGamePointsService
    {
        private readonly IDeletableEntityRepository<GamePoint> gamePointRepository;
        private readonly IRepository<GamePointGameType> gamePointGameTypeRepository;
        private readonly IDeletableEntityRepository<GameType> gameTypeRepository;

        public GamePointsService(IDeletableEntityRepository<GamePoint> gamePointRepository, IRepository<GamePointGameType> gamePointGameTypeRepository, IDeletableEntityRepository<GameType> gameTypeRepository)
        {
            this.gamePointRepository = gamePointRepository;
            this.gamePointGameTypeRepository = gamePointGameTypeRepository;
            this.gameTypeRepository = gameTypeRepository;
        }

        public async Task Add(string name, string description)
        {
            var gamePoint = new GamePoint
            {
                Name = name,
                Description = description,
            };

            await this.gamePointRepository.AddAsync(gamePoint);
            await this.gamePointRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.gamePointRepository.AllAsNoTracking().To<T>().ToList();
        }

        public IEnumerable<T> GetAll<T>(int gameId)
        {
            return this.gamePointGameTypeRepository.AllAsNoTracking().Where(gpgt => gpgt.GameType.Games.Any(g => g.Id == gameId)).To<T>().ToList();
        }

        public bool IsDuplicateName(string name)
        {
            return this.gamePointRepository.AllAsNoTracking().Any(gp => gp.Name == name);
        }
    }
}
