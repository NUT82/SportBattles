namespace SportBattles.Services.Data
{
    using System.Collections.Generic;

    public interface IMatchesService
    {
        public IEnumerable<T> GetAllByGameId<T>(int gameId);
    }
}
