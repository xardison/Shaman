namespace Shaman.Common.Extension
{
    public static class LogExt
    {
        public static void LogDestroy(this object obj)
        {
#if DEBUG
            Log.Debug("DESTROY - " + obj.GetType().Name);
#endif
        }
    }
}