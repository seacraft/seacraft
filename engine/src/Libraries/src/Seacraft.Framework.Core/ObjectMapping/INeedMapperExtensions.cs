// Copyright 2023 Seacraft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \u201CSoftware\u201D), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and\/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED \u201CAS IS\u201D, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seacraft.Framework.Core.ObjectMapping
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
