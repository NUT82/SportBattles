namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGamesService
    {
        public IEnumerable<T> GetAll<T>();

        public Task Add(string name, string description);

        public bool DuplicateName(string name);
    }
}
