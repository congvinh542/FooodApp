using ClickBuy_Api.DTOs.Models;
using ClickBuy_Api.DTOs.Queries.Base;
using System.Linq.Expressions;

namespace ClickBuy_Api.SDK.Extensions
{
    public static class QueryFillter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="source"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyFilter<T, TFilter>(this IQueryable<T> source, BaseFilter<TFilter> query)
        where TFilter : class
        {

            if (query.Entity == null)
            {
                return source;
            }
            /// <summary>
            /// <para>var parameter = Expression.Parameter(typeof(T), "e");</para>
            /// <para>var predicate = BinaryExpression.Equal(Expression.Constant(null), Expression.Constant(null));</para>
            /// </summary>
            var parameter = Expression.Parameter(typeof(T), "e");
            var predicate = BinaryExpression.Equal(Expression.Constant(null), Expression.Constant(null));

            foreach (var property in typeof(TFilter).GetProperties())
            {

                var value = property.GetValue(query.Entity);
                if (value == null)
                {
                    continue;
                }
                var propertyExpr = Expression.Property(parameter, property.Name);

                Expression conditionExpr;
                if (value is string stringValue)
                {
                    var methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    conditionExpr = Expression.Call(propertyExpr, methodInfo, Expression.Constant(stringValue));

                }
                // check value is nullble Guid
                else if (value is Guid guidValue)
                {
                    var guidExpr = Expression.Constant(guidValue, typeof(Guid?));
                    conditionExpr = Expression.Equal(propertyExpr, guidExpr);
                }
                else if (value is bool boolValue)
                {
                    var boolExpr = Expression.Constant(boolValue, typeof(bool?));
                    conditionExpr = Expression.Equal(propertyExpr, boolExpr);
                }
                else if (value is RangeFilter<int?> intRangeValue)
                {
                    if (intRangeValue.FromValue.HasValue && intRangeValue.ToValue.HasValue)
                    {
                        var fromIntExpr = Expression.Constant(intRangeValue.FromValue.Value, typeof(int?));
                        var toIntExpr = Expression.Constant(intRangeValue.ToValue.Value, typeof(int?));
                        conditionExpr = Expression.AndAlso(
                        Expression.GreaterThanOrEqual(propertyExpr, fromIntExpr),
                        Expression.LessThanOrEqual(propertyExpr, toIntExpr));
                    }
                    if (intRangeValue.FromValue.HasValue && !intRangeValue.ToValue.HasValue)
                    {
                        var fromIntExpr = Expression.Constant(intRangeValue.FromValue.Value, typeof(int?));
                        conditionExpr = Expression.GreaterThanOrEqual(propertyExpr, fromIntExpr);
                    }
                    if (!intRangeValue.FromValue.HasValue && intRangeValue.ToValue.HasValue)
                    {
                        var toIntExpr = Expression.Constant(intRangeValue.ToValue.Value, typeof(int?));
                        conditionExpr = Expression.LessThanOrEqual(propertyExpr, toIntExpr);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (value is RangeFilter<DateTime?> dateRangeValue)
                {
                    if (dateRangeValue.FromValue.HasValue && dateRangeValue.ToValue.HasValue)
                    {
                        var fromDateExpr = Expression.Constant(dateRangeValue.FromValue.Value.Date, typeof(DateTime?));
                        var toDateExpr = Expression.Constant(dateRangeValue.FromValue.Value.Date, typeof(DateTime?));
                        conditionExpr = Expression.AndAlso(
                        Expression.GreaterThanOrEqual(propertyExpr, fromDateExpr),
                        Expression.LessThanOrEqual(propertyExpr, toDateExpr));
                    }
                    if (dateRangeValue.FromValue.HasValue && !dateRangeValue.ToValue.HasValue)
                    {
                        var fromDateExpr = Expression.Constant(dateRangeValue.FromValue.Value.Date, typeof(DateTime?));
                        conditionExpr = Expression.GreaterThanOrEqual(propertyExpr, fromDateExpr);
                    }
                    if (!dateRangeValue.FromValue.HasValue && dateRangeValue.ToValue.HasValue)
                    {
                        var toDateExpr = Expression.Constant(dateRangeValue.ToValue.Value.Date, typeof(DateTime?));
                        conditionExpr = Expression.LessThanOrEqual(propertyExpr, toDateExpr);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
                predicate = Expression.AndAlso(predicate, conditionExpr);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
            return source.Where(lambda);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="colums"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByColums<T>(this IQueryable<T> source, List<string> colums, bool isDesc)
        {

            if (colums == null || colums.Count == 0)
            {
                return source;
            }
            bool isExistProperty = colums.Any(x => typeof(T).GetProperties().Any(y => y.Name == x));
            if (!isExistProperty)
            {
                return source;
            }
            var parameter = Expression.Parameter(typeof(T), "e");
            var propertyExpr = Expression.Property(parameter, colums[0]);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyExpr, typeof(object)), parameter);
            var method = isDesc ? "OrderByDescending" : "OrderBy";
            var result = source.Provider.CreateQuery<T>(
                Expression.Call(
                    typeof(Queryable),
                    method,
                    new Type[] { source.ElementType, lambda.Body.Type },
                    source.Expression,
                    lambda));
            for (int i = 1; i < colums.Count; i++)
            {
                propertyExpr = Expression.Property(parameter, colums[i]);
                lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyExpr, typeof(object)), parameter);
                method = isDesc ? "ThenByDescending" : "ThenBy";
                result = result.Provider.CreateQuery<T>(
                    Expression.Call(
                        typeof(Queryable),
                        method,
                        new Type[] { source.ElementType, lambda.Body.Type },
                        result.Expression,
                        lambda));
            }
            return result;
        }
    }
}
