namespace SportBattles.Services.Data
{
    using System.Threading.Tasks;

    public interface ITennisPlayersService
    {
        public Task<int> Add(string name, string countryName);
    }
}
