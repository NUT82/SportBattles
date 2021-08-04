namespace SportBattles.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using SportBattles.Common;
    using SportBattles.Data.Models;

    internal class GameTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.GameTypes.Any())
            {
                return;
            }

            var gameType = new GameType
                {
                    Name = "Correct score",
                    Description = "Predict the scoreline of each match and you’ll accumulate points depending on how close you are to the correct one.",
                };

            gameType.GamePoints.Add(new GamePoint
            {
                Name = "Exact scoreline",
                Value = 5,
                Description = "Predicting the final result of the regular game time perfectly",
            });

            gameType.GamePoints.Add(new GamePoint
            {
                Name = "Goal difference",
                Value = 3,
                Description = "Correctly predicting the difference of the goals scored by the teams (when also predicting the outcome correctly)",
            });

            gameType.GamePoints.Add(new GamePoint
            {
                Name = "Goals scored by one of the teams",
                Value = 2,
                Description = "Correctly predicting the number of goals scored by one of the teams",
            });

            gameType.GamePoints.Add(new GamePoint
            {
                Name = "Outcome",
                Value = 1,
                Description = "Predicting the winning team or draw correctly",
            });

            await dbContext.GameTypes.AddAsync(gameType);
            await dbContext.SaveChangesAsync();
        }
    }
}
