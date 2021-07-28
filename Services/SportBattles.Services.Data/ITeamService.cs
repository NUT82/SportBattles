namespace SportBattles.Services.Data
{
    using System.Threading.Tasks;

    public interface ITeamService
    {
        public Task<int> Add(string name, string emblemUrl, string country);
    }
}
