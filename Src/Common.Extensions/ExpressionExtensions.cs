using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Extensions
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<T, TReturn>(this Expression<Func<T, TReturn>> expression)
        {
            PropertyInfo pi = GetPropertyInfo(expression);
            return pi.Name;
        }

        public static TReturn GetPropertyValue<T, TReturn>(this Expression<Func<T, TReturn>> expression, object obj)
        {
            PropertyInfo pi = GetPropertyInfo(expression);
            return (TReturn)pi.GetValue(obj);
        }

        private static PropertyInfo GetPropertyInfo<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException("Expression is not a property", "expression");

            var pi = member.Member as PropertyInfo;
            if (pi == null)
                throw new ArgumentException("Expression is not a property", "expression");

            return pi;
        }

        public static string GetMethodName<T>(this Expression<Action<T>> expression)
        {
            var method = expression.Body as MethodCallExpression;
            if (method == null)
                throw new ArgumentException("Expression is not a method", "expression");

            return method.Method.Name;
        }

    }
}
