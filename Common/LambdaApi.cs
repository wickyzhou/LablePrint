using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Expression = System.Linq.Expressions.Expression;

namespace Common
{
    public static class LambdaApi
    {

            /// <summary>
            /// base condition join
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static Expression<Func<T, bool>> True<T>() { return f => true; }

            /// <summary>
            /// base condition join 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static Expression<Func<T, bool>> False<T>() { return f => false; }


            /// <summary>
            /// And condition join
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="exp_left"></param>
            /// <param name="exp_right"></param>
            /// <returns></returns>
            public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
            {
                var candidateExpr = Expression.Parameter(typeof(T), "candidate");
                var parameterReplacer = new ParameterReplacer(candidateExpr);
                var left = parameterReplacer.Replace(exp_left.Body);
                var right = parameterReplacer.Replace(exp_right.Body);
                var body = Expression.And(left, right);
                return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
            }

            /// <summary>
            /// Or condition join
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="exp_left"></param>
            /// <param name="exp_right"></param>
            /// <returns></returns>
            public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
            {
                var candidateExpr = Expression.Parameter(typeof(T), "candidate");
                var parameterReplacer = new ParameterReplacer(candidateExpr);
                var left = parameterReplacer.Replace(exp_left.Body);
                var right = parameterReplacer.Replace(exp_right.Body);
                var body = Expression.Or(left, right);
                return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
            }


      
    }
    internal class ParameterReplacer : ExpressionVisitor
    {
        public ParameterReplacer(ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }
        public ParameterExpression ParameterExpression { get; private set; }

        public Expression Replace(Expression expr)
        {
            return this.Visit(expr);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            return this.ParameterExpression;
        }
    }
}

