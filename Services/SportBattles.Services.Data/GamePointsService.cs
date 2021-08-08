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

        public GamePointsService(IDeletableEntityRepository<GamePoint> gamePointRepository)
        {
            this.gamePointRepository = gamePointRepository;
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
            return this.gamePointRepository.AllAsNoTracking().OrderBy(gp => gp.Name).To<T>().ToList();
        }

        public bool IsDuplicateName(string name)
        {
            return this.gamePointRepository.AllAsNoTracking().Any(gp => gp.Name == name);
        }
    }
}
