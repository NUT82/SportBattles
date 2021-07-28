namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Services.Mapping;

    public class SportsService : ISportsService
    {
        private readonly IDeletableEntityRepository<Sport> sportRepository;

        public SportsService(IDeletableEntityRepository<Sport> sportRepository)
        {
            this.sportRepository = sportRepository;
        }

        public async Task Add(string name)
        {
            var sport = this.sportRepository.All().Where(s => s.Name == name).FirstOrDefault();
            if (sport != null)
            {
                return;
            }

            var newSport = new Sport
            {
                Name = name,
            };

            await this.sportRepository.AddAsync(newSport);
            await this.sportRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.sportRepository.AllAsNoTracking().OrderBy(s => s.Name).To<T>().ToList();
        }
    }
}
