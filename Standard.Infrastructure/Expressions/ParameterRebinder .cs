using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Standard.Infrastructure.Expressions
{
    /// <summary>
    /// 重建表达式参数
    /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression expr)
        {
            return new ParameterRebinder(map).Visit(expr);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {

            if (map.TryGetValue(node, out ParameterExpression replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }
    }
}
