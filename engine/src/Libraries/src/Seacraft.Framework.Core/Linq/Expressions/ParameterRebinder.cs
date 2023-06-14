// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ParameterRebinder.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Seacraft.Framework.Core.Linq.Expressions
{
    public class ParameterRebinder: ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = (map ?? new Dictionary<ParameterExpression, ParameterExpression>());
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression? parameterExpression;
            if (this.map.TryGetValue(p, out parameterExpression))
            {
                p = parameterExpression;
            }
            return base.VisitParameter(p);
        }
    }
}
