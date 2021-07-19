namespace SportBattles.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

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

        public IEnumerable<FootballLeague> GetFootballCountriesAndTournaments(string jsonFileName)
        {
            var jsonObj = JObject.Parse(File.ReadAllText(jsonFileName));
            var countries = jsonObj["Ccg"].ToObject<IEnumerable<FootballLeague>>();

            return countries;
        }

        public IEnumerable<FootballMatch> GetFootballMatches(DateTime startDate, DateTime endDate, string country = null, string tournament = null)
        {
            var matches = new List<FootballMatch>();
            while (startDate <= endDate)
            {
                var currDate = startDate.ToString("yyyyMMdd");
                if (!File.Exists(@"wwwroot/json/Football" + currDate + ".json"))
                {
                    startDate = startDate.AddDays(1);
                    continue;
                }

                var jsonObj = JObject.Parse(File.ReadAllText(@"wwwroot/json/Football" + currDate + ".json"));
                matches.AddRange(jsonObj["Stages"].ToObject<IEnumerable<FootballMatch>>().ToList());
                startDate = startDate.AddDays(1);
            }

            if (country == null)
            {
                return matches;
            }

            if (tournament == null)
            {
                return matches.Where(m => m.Country == country);
            }

            return matches.Where(m => m.Country == country && m.Tournament == tournament);
        }
    }
}
