using System;
using System.Collections.Generic;

namespace Shaman.Common.Extension
{
    public static class DisposableExt
    {
        public static void AddTo(this IDisposable disp, ICollection<IDisposable> list)
        {
            list.Add(disp);
        }

        public static void DisposeAll(this ICollection<IDisposable> list)
        {
            list.DoForEach(x => x.Dispose());
        }

        public static IList<IDisposable> ToListDisp(this IDisposable disp)
        {
            return new List<IDisposable> { disp };
        }
    }
}