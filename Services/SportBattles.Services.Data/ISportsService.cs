namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SportBattles.Data.Models;

    public interface ISportsService
    {
        IEnumerable<T> GetAll<T>();

        Task Add(string name);
    }
}
