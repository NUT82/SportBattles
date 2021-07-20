namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITournamentsService
    {
        IEnumerable<T> GetTournamentsForSportInCountry<T>(int sportId, int countryId);

        Task<int> AddNewTournamentToSportInCountry(int sportId, int countryId, string name);
    }
}
