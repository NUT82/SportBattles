namespace SportBattles.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;

    using SportBattles.Data.Models;
    using SportBattles.Services;

    public class TennisSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Sports.Any(s => s.Name == "Tennis"))
            {
                return;
            }

            var jsonFilePath = @"wwwroot/json/TennisLeagues.json";
            var tennisTournaments = serviceProvider.GetService<ILiveScoreApi>().GetCountriesAndTournaments(jsonFilePath);

            var tennisSport = dbContext.Sports.Where(s => s.Name == "Tennis").FirstOrDefault();
            if (tennisSport == null)
            {
                tennisSport = new Sport { Name = "Tennis" };
                await dbContext.Sports.AddAsync(tennisSport);
            }

            foreach (var item in tennisTournaments)
            {
                var country = new Country
                {
                    Name = item.Country,
                };

                country.Sports.Add(tennisSport);

                foreach (var tournament in item.Tournaments)
                {
                    if (country.Tournaments.Any(t => string.Equals(t.Name, tournament.Name, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }

                    var newTournament = new Tournament
                    {
                        Country = country,
                        Sport = tennisSport,
                        Name = tournament.Name,
                    };
                    country.Tournaments.Add(newTournament);
                }

                await dbContext.Countries.AddAsync(country);
            }
        }
    }
}
