namespace SportBattles.Services.TennisPlayerPictureScraper
{
    using System.Net;

    using AngleSharp;
    using Newtonsoft.Json.Linq;

    public class TennisExplorerScraperService : ITennisExplorerScraperService
    {
        private const string SearchPlayerUrl = "http://www.tennisexplorer.com/res/ajax/search.php?s={0}";
        private const string PlayerUrl = "http://www.tennisexplorer.com/player/{0}/";
        private readonly IBrowsingContext context;

        public TennisExplorerScraperService()
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
        }

        public string GetTennisPlayerPictureUrl(string name)
        {
            var playerUrl = this.GetTennisPlayerUrl(name);
            if (playerUrl == null)
            {
                return null;
            }

            var url = string.Format(PlayerUrl, playerUrl);

            var document = this.context
                .OpenAsync(url)
                .GetAwaiter()
                .GetResult();

            if (document.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var src = document.QuerySelector(".img img")?.GetAttribute("src");
            if (src == null)
            {
                return null;
            }

            src = $"https://www.tennisexplorer.com{src}";

            return src;
        }

        private string GetTennisPlayerUrl(string name)
        {
            var url = string.Format(SearchPlayerUrl, name);

            var document = this.context
                .OpenAsync(url)
                .GetAwaiter()
                .GetResult();

            if (document.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var start = document.DocumentElement.TextContent.IndexOf("[{") + 1;
            if (start <= 0)
            {
                return null;
            }

            var end = document.DocumentElement.TextContent.IndexOf("}]") + 1;
            var text = document.DocumentElement.TextContent[start..end];
            var jsonObj = JObject.Parse(text);
            var playerUrl = jsonObj["url"].ToObject<string>();

            return playerUrl;
        }
    }
}
