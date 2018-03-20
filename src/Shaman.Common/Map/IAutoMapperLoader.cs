using System;
using System.Collections.Generic;

namespace Shaman.Common.Map
{
    public interface IAutoMapperLoader
    {
        IList<Type> GetAutoMapperProfileTypes();
    }
}