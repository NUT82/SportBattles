namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMatchesService
    {
        public IEnumerable<T> GetAllByGameId<T>(int gameId);

        public IDictionary<int, bool> GetMatchesDoublePointsByGameId(int gameId);

        public Task ChangeDoublePoints(int matchId, int gameId);
    }
}
