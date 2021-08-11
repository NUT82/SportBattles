namespace SportBattles.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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
                    Name = "Football correct score",
                    Description = "Predict the scoreline of each match and you’ll accumulate points depending on how close you are to the correct one.",
                };

            gameType.GamePoints.Add(new GamePointGameType
            {
                GamePoint = new GamePoint
                {
                    Name = "Exact scoreline",
                    Description = "Predicting the final result of the regular game time perfectly",
                },
                Value = 5,
            });

            gameType.GamePoints.Add(new GamePointGameType
            {
                GamePoint = new GamePoint
                {
                    Name = "Goal difference",
                    Description = "Correctly predicting the difference of the goals scored by the teams (when also predicting the outcome correctly)",
                },
                Value = 3,
            });

            gameType.GamePoints.Add(new GamePointGameType
            {
                GamePoint = new GamePoint
                {
                    Name = "Goals scored by one of the teams",
                    Description = "Correctly predicting the number of goals scored by one of the teams",
                },
                Value = 2,
            });

            gameType.GamePoints.Add(new GamePointGameType
            {
                GamePoint = new GamePoint
                {
                    Name = "Outcome",
                    Description = "Predicting the winning team or draw correctly",
                },
                Value = 1,
            });

            var gameTypeOverUnder = new GameType
            {
                Name = "Football over/under 2.5 goals",
                Description = "You have to guess more or less than 2.5 goals will be scored in the match - no matter who the winner is",
            };

            gameTypeOverUnder.GamePoints.Add(new GamePointGameType
            {
                GamePoint = new GamePoint
                {
                    Name = "Over/Under 2.5 goals",
                    Description = "Points are awarded if you have correctly predicted more / less than 2.5 goals in the match (no matter who is the winner)",
                },
                Value = 1,
            });

            var gameTypeTennis = new GameType
            {
                Name = "Tennis exact sets",
                Description = "Predicting the exact sets result of tennis matches",
            };

            gameTypeTennis.GamePoints.Add(new GamePointGameType
            {
                GamePoint = new GamePoint
                {
                    Name = "Exact sets",
                    Description = "Predicting the final result in of tennis match perfectly",
                },
                Value = 3,
            });

            gameTypeTennis.GamePoints.Add(new GamePointGameType
            {
                GamePoint = new GamePoint
                {
                    Name = "Tennis player winner",
                    Description = "Predicting the winning tennis player correctly",
                },
                Value = 1,
            });

            await dbContext.GameTypes.AddAsync(gameType);
            await dbContext.SaveChangesAsync();
        }
    }
}
