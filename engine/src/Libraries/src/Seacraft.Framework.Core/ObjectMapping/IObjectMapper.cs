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
    public interface IObjectMapper
    {
        /// <summary>
        /// 将一个对象转换为另一个对象。创建的新对象 <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">目标对象的类型</typeparam>
        /// <param name="source">源对象</param>
        TDestination MapTo<TDestination>(object source);

        /// <summary>
        /// 从源对象到现有目标对象的映射
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="destination">目标对象</param>
        /// <returns><paramref name="destination"/> 映射后的对象</returns>
        TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination);

        IEnumerable<TDestination> MapToEnumerable<TSource, TDestination>(IEnumerable<TSource> source);


        IEnumerable<TDestination> MapToCollection<TSource, TDestination>(ICollection<TSource> source);

        /// <summary>
        /// 从源对象到现有目标对象的映射
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="sourceType">源类型</param>
        /// <param name="destinationType">目标类型</param>
        /// <returns><paramref name="object"/> 映射后的对象</returns>
        object MapTo(object source, Type sourceType, Type destinationType);


        /// <summary>
        /// 将一个对象转换为另一个对象。创建的新对象 Queryable
        /// </summary>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源Queryable</param>
        IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source);
    }
}
