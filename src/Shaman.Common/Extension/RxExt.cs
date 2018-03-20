using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Shaman.Common.Extension
{
    public static class RxExt
    {
        public static IScheduler UiScheduler { get; private set; }

        public static void SetUiScheduler(IScheduler uiScheduler)
        {
            UiScheduler = uiScheduler;
        }

        public static IObservable<TSource> ObserveOnUiScheduler<TSource>(this IObservable<TSource> source)
        {
            return source.ObserveOn(UiScheduler);
        }
    }
}