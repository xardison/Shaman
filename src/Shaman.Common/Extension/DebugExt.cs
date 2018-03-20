using System;
using System.Diagnostics;

namespace Shaman.Common.Extension
{
    public static class DebugExt
    {
        public static IDisposable Timer
        {
            get { return new Timerx(); }
        }

        internal class Timerx : IDisposable
        {
            //TODO threadstatic
            private readonly Stopwatch _sw = new Stopwatch();

            public Timerx()
            {
                Log.Debug("Timer start");
                _sw.Start();
            }

            public void Dispose()
            {
                _sw.Stop();
                Log.Debug($"Timer end | {_sw.Elapsed} ticks: {_sw.ElapsedTicks}");
                _sw.Reset();
            }
        }
    }
}
