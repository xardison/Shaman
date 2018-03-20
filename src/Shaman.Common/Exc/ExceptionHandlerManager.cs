using System;
using System.Collections.Generic;
using Shaman.Common.Extension;

namespace Shaman.Common.Exc
{
    public interface IExceptionHandlersManager
    {
        bool Handle(Exception exception);
    }

    internal class ExceptionHandlersManager : IExceptionHandlersManager
    {
        private readonly IList<IExceptionHandler> _exceptionHandlers;

        public ExceptionHandlersManager(IList<IExceptionHandler> exceptionHandlers)
        {
            _exceptionHandlers = exceptionHandlers;
        }

        public bool Handle(Exception exception)
        {
            if (exception is AggregateException ae)
            {
                var result = true;

                ae.InnerExceptions.DoForEach(e => { result = result && Handle(e); });

                return result;
            }

            foreach (var exceptionHandler in _exceptionHandlers)
            {
                if (exceptionHandler.Handle(exception))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
