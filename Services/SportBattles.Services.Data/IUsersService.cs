namespace SportBattles.Services.Data
{
    using System.Collections.Generic;

    public interface IUsersService
    {
        public IEnumerable<T> GetTopPlayers<T>(int count);
    }
}
