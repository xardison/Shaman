using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Splat;

namespace Shaman.Common
{
    internal class ShamanDependencyResolver : IMutableDependencyResolver
    {
        private IKernel _kernel;

        public ShamanDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Dispose()
        {
            _kernel = null;
        }

        public object GetService(Type serviceType, string contract = null)
        {
            // Type-Implementation : returned value
            // 1-M : return last registred;
            // M-1 : return new instance
            // M-1 (in 1 bind) : return single instance
            return (GetServices(serviceType, contract) ?? Enumerable.Empty<object>()).LastOrDefault();

            // HACK юзаю 1 вариант т.к. хочу подменить реактивовскую реализацию IViewLocator на свою
            // или всетаки замутить отдельный кеш, чтобы тут запрашивать одну реализацию а не все и получать в рунтайме ошибку, если такое будет?
            /*
             * if (contract != null) return _kernel.Get(serviceType, contract);
             * var item = _kernel.Get(serviceType);
             * return item;
            */
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            if (contract != null) return _kernel.GetAll(serviceType, contract);
            var items = _kernel.GetAll(serviceType);
            var list = items.ToList();
            return list;
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            var binding = _kernel.Bind(serviceType).ToMethod(_ => factory());
            if (contract != null) binding.Named(contract);
        }

        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }
    }
}