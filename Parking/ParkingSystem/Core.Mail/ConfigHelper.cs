using System.Configuration;

namespace Core.Mail
{
    internal static class ConfigHelper
    {
        public static string fromAddress => ConfigurationManager.AppSettings["fromAddress"];
        public static string fromName => ConfigurationManager.AppSettings["fromName"];
        public static string fromPassword => ConfigurationManager.AppSettings["fromPassword"];
        public static string fromSubject => ConfigurationManager.AppSettings["fromSubject"];

    }
}
