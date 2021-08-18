namespace SportBattles.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SportBattles.Data.Models;

    public interface ITournamentsService
    {
        IEnumerable<T> GetAllForSportInCountry<T>(int sportId, int countryId);
    }
}
