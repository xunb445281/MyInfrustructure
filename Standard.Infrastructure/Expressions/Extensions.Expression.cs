using Standard.Infrastructure.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Standard.Infrastructure
{
    /// <summary>
    /// expression表达式扩展
    /// </summary>
    public static partial class Extensions
    {
        public static Expression<Func<T, bool>> Compose<T>(this Expression<Func<T, bool>> first, string property, object value, Func<Expression, Expression, Expression> merge)
        {
            var type = typeof(T);
            var param = Expression.Parameter(type, "s");
            var memberExpression = Expression.Property(param, property);
            var constantExpression = Expression.Constant(value);
            BinaryExpression expression = Expression.Equal(memberExpression, constantExpression);

            var map = first.Parameters.Select((f, i) => new { f, s = param }).ToDictionary(p => p.s, p => p.f);

            var secondBody = ParameterRebinder.ReplaceParameters(map, expression);

            return Expression.Lambda<Func<T, bool>>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
}
