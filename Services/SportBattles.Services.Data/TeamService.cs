namespace SportBattles.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;

    public class TeamService : ITeamService
    {
        private readonly IDeletableEntityRepository<Team> teamRepository;
        private readonly IDeletableEntityRepository<Country> countryRepository;

        public TeamService(
            IDeletableEntityRepository<Team> teamRepository,
            IDeletableEntityRepository<Country> countryRepository)
        {
            this.teamRepository = teamRepository;
            this.countryRepository = countryRepository;
        }

        public async Task<int> Add(string name, string emblemUrl, string countryName)
        {
            var team = this.teamRepository.All().FirstOrDefault(t => t.Name == name && t.Country.Name == countryName);

            if (team == null)
            {
                var countryId = this.countryRepository.AllAsNoTracking().Where(c => c.Name == countryName).Select(c => c.Id).FirstOrDefault();
                team = new Team
                {
                    Name = name,
                    CountryId = countryId,
                    EmblemUrl = emblemUrl,
                };

                await this.teamRepository.AddAsync(team);
                await this.teamRepository.SaveChangesAsync();
            }

            return team.Id;
        }
    }
}
