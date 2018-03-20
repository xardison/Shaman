using System;
using log4net;
using log4net.Config;

namespace Shaman.Common
{
    public class Log
    {
        static Log()
        {
            XmlConfigurator.Configure();
        }

        private static ILog _logger;

        private static ILog Logger => _logger ?? (_logger = LogManager.GetLogger("LOGGER"));

        public static void Debug(Exception exception, string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Debug(message, exception);
            }
            else
            {
                Logger.Debug(string.Format(message, args), exception);
            }
        }
        public static void Debug(string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Debug(message);
            }
            else
            {
                Logger.Debug(string.Format(message, args));
            }
        }

        public static void Info(Exception exception, string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Info(message, exception);
            }
            else
            {
                Logger.Info(string.Format(message, args), exception);
            }
        }
        public static void Info(string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Info(message);
            }
            else
            {
                Logger.Info(string.Format(message, args));
            }
        }

        public static void Warn(Exception exception, string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Warn(message, exception);
            }
            else
            {
                Logger.Warn(string.Format(message, args), exception);
            }
        }
        public static void Warn(string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Warn(message);
            }
            else
            {
                Logger.Warn(string.Format(message, args));
            }
        }

        public static void Error(Exception exception, string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Error(message, exception);
            }
            else
            {
                Logger.Error(string.Format(message, args), exception);
            }
        }
        public static void Error(string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Error(message);
            }
            else
            {
                Logger.Error(string.Format(message, args));
            }
        }

        public static void Fatal(Exception exception, string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Fatal(message, exception);
            }
            else
            {
                Logger.Fatal(string.Format(message, args), exception);
            }
        }
        public static void Fatal(string message, params object[] args)
        {
            if (args.Length == 0)
            {
                Logger.Fatal(message);
            }
            else
            {
                Logger.Fatal(string.Format(message, args));
            }
        }

        /*
          public enum LogLevel
          {
            Debug = 1,
            Info = 2,
            Warn = 3,
            Error = 4,
            Fatal = 5,
          }
        */
    }
}