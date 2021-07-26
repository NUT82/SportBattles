namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGamesService
    {
        public IEnumerable<T> GetAllGames<T>();

        public IEnumerable<T> GetAllTypes<T>();

        public Task AddGame(string name, int typeId);

        public Task DeleteGame(int gameId);

        public Task FinishGame(int gameId);

        public Task AddType(string name, string description);

        public bool DuplicateGame(string name, int typeId);

        public bool DuplicateTypeName(string name);
    }
}
