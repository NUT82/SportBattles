namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGamesService
    {
        public IEnumerable<T> GetAll<T>();

        public IEnumerable<T> GetUserGames<T>(string userId);

        public IEnumerable<T> GetAllStarted<T>();

        public IEnumerable<T> GetAllTypes<T>();

        public Task Join(int gameId, string userId);

        public Task Add(string name, int typeId);

        public Task AddMatches(int gameId, IEnumerable<FootballMatch> footballMatches);

        public Task Delete(int gameId);

        public Task ChangeStatus(int gameId);

        public Task AddType(string name, string description);

        public bool IsDuplicate(string name, int typeId);

        public bool IsDuplicateTypeName(string name);
    }
}
