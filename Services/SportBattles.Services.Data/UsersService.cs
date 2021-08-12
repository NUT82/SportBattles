namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<T> GetTopPlayers<T>(int count)
        {
            return this.userRepository.AllAsNoTracking().OrderByDescending(u => u.Predictions.Sum(p => p.Points) + u.TennisPredictions.Sum(p => p.Points)).ThenBy(u => u.Predictions.Count + u.TennisPredictions.Count).Take(count).To<T>().ToList();
        }

        public IEnumerable<T> GetTopPlayersInGame<T>(int gameId, int count)
        {
            return this.userRepository.AllAsNoTracking().Where(u => u.Games.Any(g => g.Id == gameId)).OrderByDescending(u => u.Predictions.Where(p => p.GameId == gameId).Sum(p => p.Points) + u.TennisPredictions.Where(p => p.GameId == gameId).Sum(p => p.Points)).ThenBy(u => u.Predictions.Where(p => p.GameId == gameId).Count() + u.TennisPredictions.Where(p => p.GameId == gameId).Count()).Take(count).To<T>().ToList();
        }
    }
}
