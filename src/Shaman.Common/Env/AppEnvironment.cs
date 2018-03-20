using System;
using System.Globalization;
using System.Reflection;

namespace Shaman.Common.Env
{
    public class AppEnvironment
    {
        static AppEnvironment()
        {
            CurrentCulture = CultureInfo.GetCultureInfo("ru-Ru");
            AppStartTime = DateTime.UtcNow;

            User = new User
            {
                Name = "unauthorized",
                AdLogin = Environment.UserName,
                PcName = Environment.MachineName,
                Email = "unknow@d00.vz"
            };

            Db = Database.GetById(DatabaseId.Unknow);

            AppVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        public static CultureInfo CurrentCulture { get; }
        public static DateTime AppStartTime { get; }
        public static string AppVersion { get; }
        public static string AppBackgroundImageUri { get; private set; }
        public static string AppIconImageUri { get; private set; }

        public static string AppName { get; private set; }
        public static string HumanAppName { get; private set; }
        public static string DevelopersMail { get; private set; }
        public static string DevelopersPhone { get; private set; }
        public static string UrlSite { get; private set; }
        public static string UrlSiteHelp { get; private set; }

        public static User User { get; private set; }
        public static Database Db { get; private set; }

        // public methods ====================================================================
        public static void SetDb(Database db)
        {
            Db = db;
        }
        public static void SetUser(User user)
        {
            User = user;
        }

        public static void SetParams(
            string appName,
            string humanAppName,
            string developersMail,
            string developersPhone,
            string urlSite,
            string urlSiteHelp)
        {
            AppName = appName;
            HumanAppName = humanAppName;
            DevelopersMail = developersMail;
            DevelopersPhone = developersPhone;
            UrlSite = urlSite;
            UrlSiteHelp = urlSiteHelp;
        }

        public static void SetBackgroundAndIconImageUri(string backgroundUri, string iconUri)
        {
            AppBackgroundImageUri = backgroundUri;
            AppIconImageUri = iconUri;
        }
    }
}