namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SportBattles.Web.ViewModels.Administration.Game;
    using SportBattles.Web.ViewModels.Game;

    public interface IGamesService
    {
        public IEnumerable<RankingViewModel> GetRanking(int gameId);

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<T> GetUserGames<T>(string userId);

        public IDictionary<int, int> UnpredictedMatchesInGameByUserCount(string userId);

        public IEnumerable<T> GetAllStarted<T>(string userId);

        public IEnumerable<T> GetTopGames<T>(int count);

        public IEnumerable<T> GetAllTypes<T>();

        public Task Join(int gameId, string userId);

        public Task Add(string name, int typeId);

        public Task AddMatches(int gameId, IEnumerable<FootballMatchServiceModel> footballMatches);

        public Task AddTennisMatches(int gameId, IEnumerable<TennisMatchServiceModel> footballMatches);

        public Task Delete(int gameId);

        public Task ChangeStatus(int gameId);

        public Task AddType(GameTypeInputModel input);

        public bool IsDuplicate(string name, int typeId);

        public bool IsDuplicateTypeName(string name);
    }
}
