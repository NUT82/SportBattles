namespace SportBattles.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using SportBattles.Common;
    using SportBattles.Services.JsonModels;

    public class LiveScoreApi : ILiveScoreApi
    {
        public async Task CreateJsonFilesForAllMatchesAsync(DateTime startDate, DateTime endDate, string apiKey, string apiHost, string category)
        {
            if (!GlobalConstants.LiveScoreApiCategories.Contains(category))
            {
                throw new ArgumentOutOfRangeException("This category is not supported by API");
            }

            var client = new HttpClient();

            while (startDate <= endDate)
            {
                var currDate = startDate.ToString("yyyyMMdd");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://livescore6.p.rapidapi.com/matches/v2/list-by-date?Category=" + category.ToLower() + "&Date=" + currDate),
                    Headers =
                {
                    { "x-rapidapi-key", apiKey },
                    { "x-rapidapi-host", apiHost },
                },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    using FileStream fileStream = File.Create(@"wwwroot/json/" + category + currDate + ".json");
                    await response.Content.CopyToAsync(fileStream);
                }

                startDate = startDate.AddDays(1);
            }
        }

        public IEnumerable<LeagueJson> GetCountriesAndTournaments(string jsonFileName)
        {
            var jsonObj = JObject.Parse(File.ReadAllText(jsonFileName));
            var countries = jsonObj["Ccg"].ToObject<IEnumerable<LeagueJson>>();

            return countries;
        }

        public IEnumerable<FootballMatchServiceModel> GetFootballMatches(DateTime startDate, DateTime endDate, string country = null, string tournament = null)
        {
            var matches = this.GetMatchesFromJsonsApi(startDate, endDate);

            if (country == null || country == "All")
            {
                return matches;
            }

            if (tournament == null || tournament == "All")
            {
                return matches.Where(m => m.Country == country);
            }

            return matches.Where(m => m.Country == country && m.Tournament == tournament);
        }

        public IEnumerable<TennisMatchServiceModel> GetTennisMatches(DateTime startDate, DateTime endDate, string country = null, string tournament = null)
        {
            var matches = this.GetTennisMatchesFromJsonsApi(startDate, endDate);

            if (country == null || country == "All")
            {
                return matches;
            }

            if (tournament == null || tournament == "All")
            {
                return matches.Where(m => m.Country == country);
            }

            return matches.Where(m => m.Country == country && m.Tournament == tournament);
        }

        private IEnumerable<FootballMatchServiceModel> GetMatchesFromJsonsApi(DateTime startDate, DateTime endDate)
        {
            var matchesJson = new List<FootballMatchJson>();
            while (startDate <= endDate)
            {
                var currDate = startDate.ToString("yyyyMMdd");
                if (!File.Exists(@"wwwroot/json/Soccer" + currDate + ".json"))
                {
                    startDate = startDate.AddDays(1);
                    continue;
                }

                var jsonObj = JObject.Parse(File.ReadAllText(@"wwwroot/json/Soccer" + currDate + ".json"));
                matchesJson.AddRange(jsonObj["Stages"].ToObject<IEnumerable<FootballMatchJson>>().ToList());
                startDate = startDate.AddDays(1);
            }

            var matches = new List<FootballMatchServiceModel>();
            foreach (var league in matchesJson)
            {
                foreach (var match in league.Events)
                {
                    matches.Add(new FootballMatchServiceModel
                    {
                        Id = match.Id,
                        Country = league.Country,
                        Tournament = league.Tournament,
                        StartTimeUTC = DateTime.ParseExact(match.StartTime, "yyyyMMddHHmmss", null).AddHours(-GlobalConstants.LiveScoreAPITimeZoneCorrection),
                        Status = match.Status,
                        HomeTeam = match.Home[0].Name,
                        HomeTeamCountry = match.Home[0].Country,
                        HomeTeamEmblemUrl = this.MakeEmblemUrl(match.Home[0].EmblemUrl),
                        AwayTeam = match.Away[0].Name,
                        AwayTeamCountry = match.Away[0].Country,
                        AwayTeamEmblemUrl = this.MakeEmblemUrl(match.Away[0].EmblemUrl),
                        HomeGoals = string.IsNullOrEmpty(match.HomeGoals) ? null : byte.Parse(match.HomeGoals),
                        AwayGoals = string.IsNullOrEmpty(match.AwayGoals) ? null : byte.Parse(match.AwayGoals),
                        HalfHomeGoals = string.IsNullOrEmpty(match.HomeGoalsFirstHalf) ? null : byte.Parse(match.HomeGoalsFirstHalf),
                        HalfAwayGoals = string.IsNullOrEmpty(match.AwayGoalsFirstHalf) ? null : byte.Parse(match.AwayGoalsFirstHalf),
                    });
                }
            }

            return matches;
        }

        private string MakeEmblemUrl(string emblemFromApi)
        {
            return emblemFromApi?.Replace("enet", @"https://es-img.enetscores.com/logos").Replace(".png", "z");
        }

        private IEnumerable<TennisMatchServiceModel> GetTennisMatchesFromJsonsApi(DateTime startDate, DateTime endDate)
        {
            var tennisMatchesJson = new List<TennisMatchJson>();
            while (startDate <= endDate)
            {
                var currDate = startDate.ToString("yyyyMMdd");
                if (!File.Exists(@"wwwroot/json/Tennis" + currDate + ".json"))
                {
                    startDate = startDate.AddDays(1);
                    continue;
                }

                var jsonObj = JObject.Parse(File.ReadAllText(@"wwwroot/json/Tennis" + currDate + ".json"));
                tennisMatchesJson.AddRange(jsonObj["Stages"].ToObject<IEnumerable<TennisMatchJson>>().ToList());
                startDate = startDate.AddDays(1);
            }

            var matches = new List<TennisMatchServiceModel>();
            foreach (var league in tennisMatchesJson)
            {
                foreach (var match in league.Events)
                {
                    matches.Add(new TennisMatchServiceModel
                    {
                        Id = match.Id,
                        Country = league.Country,
                        Tournament = league.Tournament,
                        StartTimeUTC = DateTime.ParseExact(match.StartTime, "yyyyMMddHHmmss", null).AddHours(-GlobalConstants.LiveScoreAPITimeZoneCorrection),
                        Status = match.Status,
                        HomeTeam = match.Home[0].Name,
                        HomeTeamCountry = match.Home[0].Country,
                        AwayTeam = match.Away[0].Name,
                        AwayTeamCountry = match.Away[0].Country,
                        HomeSets = string.IsNullOrEmpty(match.HomeSets) ? null : byte.Parse(match.HomeSets),
                        AwaySets = string.IsNullOrEmpty(match.AwaySets) ? null : byte.Parse(match.AwaySets),
                        Games = this.GetGames(match),
                    });

                }
            }

            return matches;
        }

        private ICollection<TennisMatchServiceModel.Game> GetGames(TennisMatchJson.Event match)
        {
            var games = new List<TennisMatchServiceModel.Game>();
            if (match.HomeGamesSet1 != null)
            {
                byte? homeGames = null;
                byte? awayGames = null;
                foreach (PropertyInfo property in match.GetType().GetProperties())
                {
                    if (property.Name.StartsWith("HomeGamesSet") || property.Name.StartsWith("AwayGamesSet"))
                    {
                        byte? value = property.GetValue(match) == null ? null : byte.Parse((string)property.GetValue(match));
                        if (value == null)
                        {
                            return games;
                        }

                        if (property.Name.Contains("Home"))
                        {
                            homeGames = value;
                        }
                        else if (property.Name.Contains("Away"))
                        {
                            awayGames = value;
                        }

                        if (homeGames != null && awayGames != null)
                        {
                            games.Add(new TennisMatchServiceModel.Game { HomeGames = homeGames, AwayGames = awayGames });
                            homeGames = null;
                            awayGames = null;
                        }
                    }
                }
            }

            return games;
        }
    }
}
