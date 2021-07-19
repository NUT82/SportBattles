namespace SportBattles.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using SportBattles.Services;

    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
    }
}
