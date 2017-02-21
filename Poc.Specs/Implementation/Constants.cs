using System.Configuration;

namespace Specs.Utilities
{
    public static class Constants
    {
        public static string BaseUrl = ConfigurationManager.AppSettings["url"];
        public static string getDriver = ConfigurationManager.AppSettings["browser"];
        public static string BaseApi = ConfigurationManager.AppSettings["api"];
    }
}
