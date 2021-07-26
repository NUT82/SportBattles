namespace SportBattles.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class GamesService : IGamesService
    {
        private readonly IDeletableEntityRepository<GameType> gameTypeRepository;
        private readonly IDeletableEntityRepository<Game> gameRepository;

        public GamesService(
            IDeletableEntityRepository<GameType> gameTypeRepository,
            IDeletableEntityRepository<Game> gameRepository)
        {
            this.gameTypeRepository = gameTypeRepository;
            this.gameRepository = gameRepository;
        }

        public async Task AddGame(string name, int typeId)
        {
            var game = new Game
            {
                Name = name,
                GameTypeId = typeId,
            };

            await this.gameRepository.AddAsync(game);
            await this.gameRepository.SaveChangesAsync();
        }

        public async Task DeleteGame(int gameId)
        {
            var game = this.gameRepository.All().FirstOrDefault(g => g.Id == gameId);
            if (game == null)
            {
                throw new KeyNotFoundException("There is no game with this ID");
            }

            this.gameRepository.Delete(game);
            await this.gameRepository.SaveChangesAsync();
        }

        public async Task FinishGame(int gameId)
        {
            var game = this.gameRepository.All().FirstOrDefault(g => g.Id == gameId);
            if (game == null)
            {
                throw new KeyNotFoundException("There is no game with this ID");
            }

            game.IsFinished = true;
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

        public bool DuplicateGame(string name, int typeId)
        {
            return this.gameRepository.AllAsNoTracking().Any(g => g.Name == name && g.GameTypeId == typeId);
        }

        public bool DuplicateTypeName(string name)
        {
            return this.gameTypeRepository.AllAsNoTracking().Any(g => g.Name == name);
        }

        public IEnumerable<T> GetAllGames<T>()
        {
            return this.gameRepository.AllAsNoTracking().OrderBy(g => g.Name).To<T>().ToList();
        }

        public IEnumerable<T> GetAllTypes<T>()
        {
            return this.gameTypeRepository.AllAsNoTracking().OrderBy(g => g.Name).To<T>().ToList();
        }
    }
}
