namespace SportBattles.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.TennisPlayerPictureScraper;

    public class TennisPlayersService : ITennisPlayersService
    {
        private readonly IDeletableEntityRepository<TennisPlayer> tennisPlayersRepository;
        private readonly IDeletableEntityRepository<Country> countryRepository;
        private readonly ITennisExplorerScraperService sofaScoreScraperService;

        public TennisPlayersService(IDeletableEntityRepository<TennisPlayer> tennisPlayersRepository, IDeletableEntityRepository<Country> countryRepository, ITennisExplorerScraperService sofaScoreScraperService)
        {
            this.tennisPlayersRepository = tennisPlayersRepository;
            this.countryRepository = countryRepository;
            this.sofaScoreScraperService = sofaScoreScraperService;
        }

        public async Task<int> Add(string name, string countryName)
        {
            var tennisPlayer = this.tennisPlayersRepository.All().FirstOrDefault(tp => tp.Name == name && tp.Country.Name == countryName);

            if (tennisPlayer == null)
            {
                var country = this.countryRepository.AllAsNoTracking().Where(c => c.Name == countryName).FirstOrDefault();
                if (country == null)
                {
                    country = new Country { Name = countryName };
                    await this.countryRepository.AddAsync(country);
                    await this.countryRepository.SaveChangesAsync();
                }

                tennisPlayer = new TennisPlayer
                {
                    Name = name,
                    CountryId = country.Id,
                    PictureUrl = this.sofaScoreScraperService.GetTennisPlayerPictureUrl(name),
                };

                await this.tennisPlayersRepository.AddAsync(tennisPlayer);
                await this.tennisPlayersRepository.SaveChangesAsync();
            }

            return tennisPlayer.Id;
        }
    }
}
