using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Security.Principal;
using System.Windows.Threading;
using Ninject;
using ReactiveUI;
using Shaman.Common.Env;
using Shaman.Common.Exc;
using Shaman.Common.Extension;
using Shaman.Common.Map;
using Splat;

namespace Shaman.Common
{
    public interface IAppBootstrapper : IDisposable
    {
        void Init();
        T GetService<T>(string contract = null);
        IEnumerable<T> GetServices<T>(string contract = null);
        void OnUnhandlerException(object sender, DispatcherUnhandledExceptionEventArgs e);
    }
    public class AppBootstrapper : IAppBootstrapper
    {
        private readonly Action<Exception> _showError;
        private IKernel _kernel;
        private bool _isInitialized;

        public AppBootstrapper(Action<Exception> showError, IScheduler scheduler = null)
        {
            _showError = showError;
            RxExt.SetUiScheduler(scheduler ?? DispatcherScheduler.Current);
        }

        public void Init()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            // HACK: вызываем стат.конструктор RxApp, чтобы реактив грузился вначале
            var tmp = RxApp.DefaultExceptionHandler;

            InitDependencyInjection();
            InitDependencyResolver();
            LoadDependencys();

            SubscribeOnException();
            SetLogLevel();

            InitAutoMapper();

            _isInitialized = true;
        }

        public void Dispose()
        {
            _kernel.Dispose();
            _kernel = null;
            _isInitialized = false;
        }

        public T GetService<T>(string contract = null)
        {
            return contract == null ? _kernel.Get<T>() : _kernel.Get<T>(contract);
        }
        public IEnumerable<T> GetServices<T>(string contract = null)
        {
            return contract == null ? _kernel.GetAll<T>() : _kernel.GetAll<T>(contract);
        }
        public void OnUnhandlerException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (!_isInitialized)
            {
                _showError(e.Exception);
                return;
            }

            RxApp.DefaultExceptionHandler.OnNext(e.Exception);
            e.Handled = true;
        }

        private void InitDependencyInjection()
        {
            _kernel = new StandardKernel();
        }
        private void InitDependencyResolver(IMutableDependencyResolver dependencyResolver = null)
        {
            if (dependencyResolver == null)
            {
                _kernel.Bind<IDependencyResolver, IMutableDependencyResolver>().To<ShamanDependencyResolver>().InSingletonScope();
                dependencyResolver = _kernel.Get<IMutableDependencyResolver>();
            }

            // При замене резолвера, зависимости из старого контейнера вливаются в контейнер нового резолвера
            Locator.Current = dependencyResolver;
        }
        private void LoadDependencys()
        {
            _kernel.Load(AppConfig.ModulesPathDefault);
            _kernel.Load(AppConfig.ModulesPath);
        }
        private void SubscribeOnException()
        {
            var handler = GetService<IObservableExceptionHandler>();
            handler.SetDefaultMethodToShowError(_showError);
            RxApp.DefaultExceptionHandler = handler;
        }

        private void SetLogLevel()
        {
            // Debug = 1,
            // Info = 2,
            // Warn = 3,
            // Error = 4,
            // Fatal = 5
#if DEBUG
            LogHost.Default.Level = LogLevel.Debug;
#else
            LogHost.Default.Level = LogLevel.Fatal;
#endif
        }
        private void InitAutoMapper()
        {
            var loaders = GetServices<IAutoMapperLoader>();
            AutoMapperAdapter.Initialize(loaders);
        }
    }
}