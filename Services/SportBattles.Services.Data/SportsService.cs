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

        public Task<Sport> AddNewSport(string name)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.sportRepository.AllAsNoTracking().To<T>().ToList();
        }
    }
}
