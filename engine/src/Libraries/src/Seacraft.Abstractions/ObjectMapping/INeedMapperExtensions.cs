// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'INeedMapperExtensions.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seacraft.Abstractions.ObjectMapping
{
    public static class INeedMapperExtensions
    {

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class, new()
        {
            return ObjectMapperFactory.GetObjectMapper().MapTo<TSource, TDestination>(source, new TDestination { });
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
           where TSource : class
           where TDestination : class, new()
        {
            return ObjectMapperFactory.GetObjectMapper().MapTo(source, destination);
        }

        public static object MapTo(this object source, Type sourceType, Type destinationType)
        {
            return ObjectMapperFactory.GetObjectMapper().MapTo(source, sourceType, destinationType);
        }


        public static IEnumerable<TDestination> MapToCollection<TSource, TDestination>(this ICollection<TSource> list)
           where TSource : class
           where TDestination : class, new()
        {
            return ObjectMapperFactory.GetObjectMapper().MapToCollection<TSource, TDestination>(list);
        }



        public static IEnumerable<TDestination> MapToEnumerable<TSource, TDestination>(this IEnumerable<TSource> list)
            where TSource : class
            where TDestination : class, new()
        {
            return ObjectMapperFactory.GetObjectMapper().MapToEnumerable<TSource, TDestination>(list);
        }

        public static IQueryable<TDestination> ProjectTo<TDestination>(this IQueryable source)
         where TDestination : class, new()
        {
            return ObjectMapperFactory.GetObjectMapper().ProjectTo<TDestination>(source);
        }
    }
}
