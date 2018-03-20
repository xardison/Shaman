namespace Shaman.Common.Env
{
    public class AppConfig
    {
        public const string LibDebugPathSegment = @"../../../";
        public const string ModulesPathDefault = @"*.dll";

#if DEBUG
        /* Пути для отладочной конфигурации */
        public const string ModulesPath = LibDebugPathSegment + @"Shaman.Bin/*.dll";
        public const string LibOuterPath = LibDebugPathSegment + @"Shaman.BinOut";
#else
        /* Пути для тестовой и релизной конфигурации */
        public const string ModulesPath = @"Shaman.Bin/*.dll";
        public const string LibOuterPath = @"Shaman.BinOut";
#endif

        public const string MailHost = "localmail.vz";
        public const int MailPort = 25;

        public const string MailShamanDev = "xardison@gmail.com";
    }
}
