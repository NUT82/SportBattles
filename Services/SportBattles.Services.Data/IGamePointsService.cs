namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGamePointsService
    {
        public IEnumerable<T> GetAll<T>();

        public IEnumerable<T> GetAll<T>(int gameId);

        public bool IsDuplicateName(string name);

        public Task Add(string name, string description);
    }
}
