// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'IQueryableExtensions.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Seacraft.Core.Linq.Expressions;

namespace Seacraft.Core.Linq
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
