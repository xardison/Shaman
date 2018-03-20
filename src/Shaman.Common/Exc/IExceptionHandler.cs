using System;

namespace Shaman.Common.Exc
{
    public interface IExceptionHandler
    {
        bool Handle(Exception exception);
    }
}
