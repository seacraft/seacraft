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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Seacraft.Framework.Core.Linq.Expressions;

namespace Seacraft.Framework.Core.Linq
{
    /// <summary>
    /// IQueryable Interface related expression extensions
    /// </summary>
    public static class IQueryableExtensions
    {
        private static readonly MethodInfo OrderByMethod;
        private static readonly MethodInfo OrderByDescendingMethod;
        private static readonly MethodInfo ThenByMethod;
        private static readonly MethodInfo ThenByDescendingMethod;
        static IQueryableExtensions()
        {
            var type = typeof(Queryable);
            OrderByMethod = type.GetMethods().Where(q => q.Name == "OrderBy" && q.IsGenericMethodDefinition && q.GetParameters().Length == 2 && q.GetGenericArguments().Length == 2)
                .FirstOrDefault();

            OrderByDescendingMethod = type.GetMethods().Where(q => q.Name == "OrderByDescending" && q.IsGenericMethodDefinition && q.GetParameters().Length == 2 && q.GetGenericArguments().Length == 2)
                .FirstOrDefault();

            ThenByMethod = type.GetMethods().Where(q => q.Name == "ThenBy" && q.IsGenericMethodDefinition && q.GetParameters().Length == 2 && q.GetGenericArguments().Length == 2)
                .FirstOrDefault();

            ThenByDescendingMethod = type.GetMethods().Where(q => q.Name == "ThenByDescending" && q.IsGenericMethodDefinition && q.GetParameters().Length == 2 && q.GetGenericArguments().Length == 2)
                .FirstOrDefault();
        }


        /// <summary>
        /// Sort the expression by field
        /// </summary>
        /// <typeparam name="IEntity"></typeparam>
        /// <param name="source">The current entity object</param>
        /// <param name="dictOrder">Sort the field dictionary</param>
        /// <returns></returns>
        public static IOrderedQueryable<IEntity> ApplyOrder<IEntity>(this IQueryable<IEntity> source
            , IDictionary<Expression<Func<IEntity, object>>, bool> dictOrder)
        {
            #region OrderBy
            IOrderedQueryable<IEntity>? orderedQueryable = null;
            if (dictOrder == null || dictOrder.Count == 0)
            {
                orderedQueryable = source as IOrderedQueryable<IEntity>;
            }
            else
            {
                foreach (var item in dictOrder)
                {
                    LambdaExpression lambdaExpr = item.Key.ToLambdaExpression();
                    MethodInfo? method = null;
                    object? obj = null;
                    if (orderedQueryable == null)
                    {
                        method = item.Value ? OrderByMethod : OrderByDescendingMethod;
                        obj = method.MakeGenericMethod(lambdaExpr.Parameters[0].Type, lambdaExpr.Body.Type).Invoke(null, new object[] { source, lambdaExpr });
                    }
                    else
                    {
                        method = item.Value ? OrderByMethod : OrderByDescendingMethod;
                        obj = method.MakeGenericMethod(lambdaExpr.Parameters[0].Type, lambdaExpr.Body.Type).Invoke(null, new object[] { orderedQueryable, lambdaExpr });
                    }
                    orderedQueryable = obj as IOrderedQueryable<IEntity>;
                }
            }
            #endregion
            return orderedQueryable;
        }



        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

    }
}
