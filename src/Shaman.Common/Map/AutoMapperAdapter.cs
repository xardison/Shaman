using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;
using Shaman.Common.Extension;

namespace Shaman.Common.Map
{
    public static class AutoMapperAdapter
    {
        private static readonly MapperConfigurationExpression MapperCfgExpr;

        static AutoMapperAdapter()
        {
            MapperCfgExpr = new MapperConfigurationExpression();
        }

        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        public static object MapTo(this object source, Type destination)
        {
            return Mapper.Map(source, destination);
        }

        public static IList<TDestination> MapTo<TDestination>(this IEnumerable source)
        {
            return (from object item in source select Mapper.Map<TDestination>(item)).ToList();
        }

        internal static void Initialize(IEnumerable<IAutoMapperLoader> loaders)
        {
            loaders.DoForEach(loader =>
            {
                var types = loader.GetAutoMapperProfileTypes();
                types.DoForEach(MapperCfgExpr.AddProfile);
            });

            Mapper.Initialize(MapperCfgExpr);
#if DEBUG
            Mapper.Configuration.AssertConfigurationIsValid();
#endif
        }
    }
}
