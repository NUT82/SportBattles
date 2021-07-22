namespace SportBattles.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using SportBattles.Common;
    using SportBattles.Services.JsonModels;

    public class LiveScoreApi : ILiveScoreApi
    {
        public async Task CreateJsonFilesForAllFootballMatchesAsync(DateTime startDate, DateTime endDate, string apiKey, string apiHost)
        {
            var client = new HttpClient();

            while (startDate <= endDate)
            {
                var currDate = startDate.ToString("yyyyMMdd");
                if (File.Exists(@"wwwroot/json/Football" + currDate + ".json"))
                {
                    startDate = startDate.AddDays(1);
                    continue;
                }

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://livescore6.p.rapidapi.com/matches/v2/list-by-date?Category=soccer&Date=" + currDate),
                    Headers =
                {
                    { "x-rapidapi-key", apiKey },
                    { "x-rapidapi-host", apiHost },
                },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    using FileStream fileStream = File.Create(@"wwwroot/json/Football" + currDate + ".json");
                    await response.Content.CopyToAsync(fileStream);
                }

                startDate = startDate.AddDays(1);
            }
        }

        public IEnumerable<FootballLeagueJson> GetFootballCountriesAndTournaments(string jsonFileName)
        {
            var jsonObj = JObject.Parse(File.ReadAllText(jsonFileName));
            var countries = jsonObj["Ccg"].ToObject<IEnumerable<FootballLeagueJson>>();

            return countries;
        }

        public IEnumerable<FootballMatch> GetFootballMatches(DateTime startDate, DateTime endDate, string country = null, string tournament = null)
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

        private IEnumerable<FootballMatch> GetMatchesFromJsonsApi(DateTime startDate, DateTime endDate)
        {
            var matchesJson = new List<FootballMatchJson>();
            while (startDate <= endDate)
            {
                var currDate = startDate.ToString("yyyyMMdd");
                if (!File.Exists(@"wwwroot/json/Football" + currDate + ".json"))
                {
                    startDate = startDate.AddDays(1);
                    continue;
                }

                var jsonObj = JObject.Parse(File.ReadAllText(@"wwwroot/json/Football" + currDate + ".json"));
                matchesJson.AddRange(jsonObj["Stages"].ToObject<IEnumerable<FootballMatchJson>>().ToList());
                startDate = startDate.AddDays(1);
            }

            var matches = new List<FootballMatch>();
            foreach (var league in matchesJson)
            {
                foreach (var match in league.Events)
                {
                    matches.Add(new FootballMatch
                    {
                        Id = match.Id,
                        Country = league.Country,
                        Tournament = league.Tournament,
                        StartTimeUTC = DateTime.ParseExact(match.StartTime, "yyyyMMddHHmmss", null).AddHours(-GlobalConstants.LiveScoreAPITimeZoneCorrection),
                        Status = match.Status,
                        HomeTeam = match.Home[0].Name,
                        AwayTeam = match.Away[0].Name,
                        HomeGoals = string.IsNullOrEmpty(match.HomeGoals) ? null : byte.Parse(match.HomeGoals),
                        AwayGoals = string.IsNullOrEmpty(match.AwayGoals) ? null : byte.Parse(match.AwayGoals),
                        HalfHomeGoals = string.IsNullOrEmpty(match.HomeGoalsFirstHalf) ? null : byte.Parse(match.HomeGoalsFirstHalf),
                        HalfAwayGoals = string.IsNullOrEmpty(match.AwayGoalsFirstHalf) ? null : byte.Parse(match.AwayGoalsFirstHalf),
                    });
                }
            }

            return matches;
        }
    }
}
