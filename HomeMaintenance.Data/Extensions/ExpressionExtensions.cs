using System;
using System.Linq.Expressions;

namespace HomeMaintenance.Data.Extensions
{
    /// <summary>
    /// Extension methods for expressions
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combines two expressions with AND logic
        /// </summary>
        /// <typeparam name="T">The type of the parameter</typeparam>
        /// <param name="first">The first expression</param>
        /// <param name="second">The second expression</param>
        /// <returns>The combined expression</returns>
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var visitor = new ReplaceParameterVisitor(parameter);
            var firstBody = visitor.Visit(first.Body);
            var secondBody = visitor.Visit(second.Body);
            var combined = Expression.AndAlso(firstBody, secondBody);
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        /// <summary>
        /// Combines two expressions with OR logic
        /// </summary>
        /// <typeparam name="T">The type of the parameter</typeparam>
        /// <param name="first">The first expression</param>
        /// <param name="second">The second expression</param>
        /// <returns>The combined expression</returns>
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var visitor = new ReplaceParameterVisitor(parameter);
            var firstBody = visitor.Visit(first.Body);
            var secondBody = visitor.Visit(second.Body);
            var combined = Expression.OrElse(firstBody, secondBody);
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            public ReplaceParameterVisitor(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _parameter;
            }
        }
    }
} 