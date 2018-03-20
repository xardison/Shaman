using Splat;

namespace Shaman.Common
{
    internal class LogSplatAdapter : Log, ILogger
    {
        public LogLevel Level { get; set; }

        public void Write(string message, LogLevel logLevel)
        {
#if !DEBUG
            return;
#endif

            if ((int)logLevel < (int)Level)
            {
                return;
            }

            switch (logLevel)
            {
                case LogLevel.Debug: Debug(message); break;
                case LogLevel.Info: Info(message); break;
                case LogLevel.Warn: Warn(message); break;
                case LogLevel.Error: Error(message); break;
                case LogLevel.Fatal: Fatal(message); break;

                default:
                    System.Diagnostics.Debug.WriteLine(message);
                    break;
            }
        }
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