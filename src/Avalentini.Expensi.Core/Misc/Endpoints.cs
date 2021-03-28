// ReSharper disable InconsistentNaming
namespace Avalentini.Expensi.Core.Misc
{
    public static class Endpoints
    {
        public const string IIS_ENDPOINT = "https://localhost:44358/";
        public const string STANDALONE_ENDPOINT = "http://localhost:5000/";

        public static bool InsideIIS() =>
            System.Environment.GetEnvironmentVariable("APP_POOL_ID") != null;

        public static string GetEndpoint() =>
            InsideIIS() ? IIS_ENDPOINT : STANDALONE_ENDPOINT;

        // Taken from https://stackoverflow.com/a/2806717/5524281
        public static string UrlCombine(string url1, string url2)
        {
            if (url1.Length == 0) {
                return url2;
            }

            if (url2.Length == 0) {
                return url1;
            }

            url1 = url1.TrimEnd('/', '\\');
            url2 = url2.TrimStart('/', '\\');

            return $"{url1}/{url2}";
        }
    }
}
