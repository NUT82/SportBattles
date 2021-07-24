namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class GamesService : IGamesService
    {
        private readonly IDeletableEntityRepository<GameType> gameTypeRepository;

        public GamesService(IDeletableEntityRepository<GameType> gameTypeRepository)
        {
            this.gameTypeRepository = gameTypeRepository;
        }

        public async Task Add(string name, string description)
        {
            var gameType = new GameType
            {
                Name = name,
                Description = description,
            };

            await this.gameTypeRepository.AddAsync(gameType);
            await this.gameTypeRepository.SaveChangesAsync();
        }

        public bool DuplicateName(string name)
        {
            return this.gameTypeRepository.AllAsNoTracking().Any(g => g.Name == name);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.gameTypeRepository.AllAsNoTracking().OrderBy(g => g.Name).To<T>().ToList();
        }
    }
}
