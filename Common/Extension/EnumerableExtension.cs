using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common
{
    public static class EnumerableExtension
    {
        public static readonly char[] DefaultSeperators = { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '=', ' ', ';', ':', '"', '\'', '{', '}', '[', ']', '\\', '|', '?', '/', ',', '<', '.', '>' };

        #region IQueryable Extensions

        /// <summary>
        /// Paging the entities
        /// </summary>
        /// <typeparam name="TEntity">Generic Entity Type</typeparam>
        /// <param name="entities"></param>
        /// <param name="skipCount">skip count before fetching page entities</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">the total count of entities, if totalCount isn't set, or is less than -1, totalCount will be reset with entities.Count()</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> Page<TEntity>(this IQueryable<TEntity> entities, int skipCount, int pageSize, int totalCount = -1)
           where TEntity : class
        {
            if (totalCount <= -1)
                totalCount = entities.Count();

            if (totalCount <= skipCount)
                skipCount = 0;

            var query = entities.Skip(skipCount).Take(pageSize);
            var i = 0;
            for (; i < skipCount; i++)
            {
                yield return null;
            }
            foreach (var entity in query)
            {
                i++;
                yield return entity;
            }
            for (; i < totalCount; i++)
            {
                yield return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="page">index from 1</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> entities, int page, int pageSize)
           where TEntity : class
        {
            return entities.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<TEntity> Sort<TEntity, Tkey>(this IQueryable<TEntity> entities, Expression<Func<TEntity, Tkey>> keySelector, bool isAscending, bool useThenBy)
           where TEntity : class
        {

            if (useThenBy && (entities as IOrderedQueryable<TEntity>) != null)
            {
                var orderedQueryable = entities as IOrderedQueryable<TEntity>;
                return isAscending
                            ? orderedQueryable.ThenBy(keySelector)
                            : orderedQueryable.ThenByDescending(keySelector);
            }
            else
            {
                return isAscending
                            ? entities.OrderBy(keySelector)
                            : entities.OrderByDescending(keySelector);
            }
        }

        /// <summary>
        /// Search the source entity list (searchPattern will be splitted by default seperators)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="searchSource"></param>
        /// <param name="searchKeyExpression"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> LikeAny<TEntity>(
            this IQueryable<TEntity> searchSource,
            Expression<Func<TEntity, string>> searchKeyExpression,
            IEnumerable<string> searchPattern)
        {
            return searchSource.LikeAny(searchKeyExpression, searchPattern, DefaultSeperators);
        }

        /// <summary>
        /// Search the source entity list (searchPattern will be splitted by seperators).
        /// But if the seperators parameter is NULL, the searchPattern won't be splitted.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="searchSource"></param>
        /// <param name="searchKeyExpression"></param>
        /// <param name="searchPattern"></param>
        /// <param name="seperators">Except seperators==NULL, others will follow the string.Split() behaviors</param>
        /// <returns></returns>
        public static IQueryable<TEntity> LikeAny<TEntity>(this IQueryable<TEntity> searchSource,
                                                           Expression<Func<TEntity, string>> searchKeyExpression,
                                                           IEnumerable<string> searchPattern,
                                                           char[] seperators)
        {
            if (searchKeyExpression == null)
                throw new ArgumentNullException("searchKeyExpression");
            if (searchPattern == null)
                throw new ArgumentNullException("searchPattern");
            if (!searchPattern.Any())
            {
                return searchSource.Take(0);
            }

            List<string> temp = new List<string>();
            foreach (var pattern in searchPattern)
            {
                if (!string.IsNullOrEmpty(pattern.Trim()))
                {
                    // I have changed this default logic, in order to sometimes we only need search the whole pattern.
                    // But, the default logic for String.Split():
                    // Although we give the parameter with: NULL, or Empty char array to String.Split()
                    // It'll split the source string with " ", which is not expected to us.(We expected search the whole pattern).
                    var tmp = seperators == null ? new string[] { pattern } : pattern.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Any())
                    {
                        foreach (var splitBySpaceCharacter in tmp)
                        {
                            if (!string.IsNullOrEmpty(splitBySpaceCharacter.Trim()))
                            {
                                temp.Add(splitBySpaceCharacter.Trim());
                            }
                        }
                    }
                }
            }
            if (!temp.Any())
            {
                return searchSource.Take(0);
            }
            else
            {
                searchPattern = temp;
            }

            var toLowerExpression = Expression.Call(searchKeyExpression.Body, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
            var notNullExpression = Expression.NotEqual(searchKeyExpression.Body, Expression.Constant(null));

            var expression = searchPattern.Select(v =>
                                                   Expression.Equal(Expression.Call(toLowerExpression, typeof(string).GetMethod("Contains"), Expression.Constant(v.ToLowerInvariant())),
                                                                    Expression.Constant(true)));

            var expressionAccumulation = expression.Aggregate((acc, exp) => Expression.OrElse(acc, exp));
            expressionAccumulation = Expression.AndAlso(notNullExpression, expressionAccumulation);

            return searchSource.Where(Expression.Lambda<Func<TEntity, bool>>(expressionAccumulation, searchKeyExpression.Parameters.Single()));
        }

        public static IQueryable<TEntity> LikeAnyOr<TEntity>(this IQueryable<TEntity> searchSource,
                                                          IEnumerable<Expression<Func<TEntity, string>>> searchKeyExpressions,
                                                          IEnumerable<string> searchPattern,
                                                          char[] seperators)
        {
            if (searchKeyExpressions == null)
            {
                throw new ArgumentNullException("searchKeyExpressions");
            }

            if (searchPattern == null)
            {
                throw new ArgumentNullException("searchPattern");
            }

            if (!searchPattern.Any())
            {
                return searchSource.Take(0);
            }

            List<string> temp = new List<string>();
            foreach (var pattern in searchPattern)
            {
                if (!string.IsNullOrEmpty(pattern.Trim()))
                {
                    // I have changed this default logic, in order to sometimes we only need search the whole pattern.
                    // But, the default logic for String.Split():
                    // Although we give the parameter with: NULL, or Empty char array to String.Split()
                    // It'll split the source string with " ", which is not expected to us.(We expected search the whole pattern).
                    var tmp = seperators == null ? new string[] { pattern } : pattern.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Any())
                    {
                        foreach (var splitBySpaceCharacter in tmp)
                        {
                            if (!string.IsNullOrEmpty(splitBySpaceCharacter.Trim()))
                            {
                                temp.Add(splitBySpaceCharacter.Trim());
                            }
                        }
                    }
                }
            }
            if (!temp.Any())
            {
                return searchSource.Take(0);
            }
            else
            {
                searchPattern = temp;
            }

            var stringComparer = StringComparer.Create(System.Globalization.CultureInfo.CurrentCulture, true);
            var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            var startWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            // var sqlLikeMethod = typeof(System.Data.Linq.SqlClient.SqlMethods).GetMethod("Like", new Type[]{typeof(string),typeof(string)});

            var accumulationResultExpression = default(Expression<Func<TEntity, bool>>);

            // As each searchKeyExpression has different parameter, we should replace all the parameters with the same one.
            foreach (var searchKeyExpression in searchKeyExpressions)
            {
                var toLowerExpression = Expression.Call(searchKeyExpression.Body, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
                var notNullExpression = Expression.NotEqual(searchKeyExpression.Body, Expression.Constant(null));

                var expression = searchPattern.Select(v =>
                                                       Expression.Equal(
                                                                        Expression.Call(searchKeyExpression.Body, containsMethod, Expression.Constant(v)),
                                                                        Expression.Constant(true)));

                var expressionAccumulation = expression.Aggregate((acc, exp) => Expression.OrElse(acc, exp));
                expressionAccumulation = Expression.AndAlso(notNullExpression, expressionAccumulation);

                var current = Expression.Lambda<Func<TEntity, bool>>(expressionAccumulation, searchKeyExpression.Parameters.Single());

                // Join the result with OR
                if (accumulationResultExpression == default(Expression<Func<TEntity, bool>>))
                {
                    accumulationResultExpression = current;
                }
                else
                {
                    accumulationResultExpression = accumulationResultExpression.Or(current);
                }
                // Expression<Func<TEntity,bool>> kk= Expression.Lambda<Func<TEntity, bool>>(accumulationResultExpression, parameterExpression);               
            }
            if (searchKeyExpressions.Count() > 0)
            {
                return searchSource.Where(accumulationResultExpression);
            }
            return searchSource;
        }

        public static IQueryable<TEntity> LikeAnyOr<TEntity>(this IQueryable<TEntity> searchSource,
                                                          IEnumerable<string> searchPattern,
                                                          char[] seperators,
                                                          params Expression<Func<TEntity, string>>[] searchKeyExpressions)
        {
            if (searchKeyExpressions == null)
                return searchSource;

            return searchSource.LikeAnyOr(searchKeyExpressions, searchPattern, seperators);
        }
        #endregion

        #region IEnumerable Extensions

        public static IEnumerable<TEntity> Page<TEntity>(this IEnumerable<TEntity> entities, int skipCount, int pageSize)
            where TEntity : class
        {
            var count = entities.Count();
            if (count <= skipCount)
                skipCount = 0;
            var query = entities.Skip(skipCount).Take(pageSize);
            var i = 0;
            for (; i < skipCount; i++)
            {
                yield return null;
            }
            foreach (var entity in query)
            {
                i++;
                yield return entity;
            }
            for (; i < count; i++)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Search the source entity list (searchPattern will be splitted by default seperators)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="searchSource"></param>
        /// <param name="searchKeyExpression"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> LikeAny<TEntity>(this IEnumerable<TEntity> searchSource,
                                                            Expression<Func<TEntity, string>> searchKeyExpression,
                                                            IEnumerable<string> searchPattern)
        {
            return searchSource.LikeAny(searchKeyExpression, searchPattern, DefaultSeperators);
        }

        /// <summary>
        /// Search the source entity list (searchPattern will be splitted by seperators).
        /// But if the seperators parameter is NULL, the searchPattern won't be splitted.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="searchSource"></param>
        /// <param name="searchKeyExpression"></param>
        /// <param name="searchPattern"></param>
        /// <param name="seperators">Except seperators==NULL, others will follow the string.Split() behaviors</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> LikeAny<TEntity>(this IEnumerable<TEntity> searchSource,
                                                           Expression<Func<TEntity, string>> searchKeyExpression,
                                                           IEnumerable<string> searchPattern,
                                                           char[] seperators)
        {
            if (searchKeyExpression == null)
                throw new ArgumentNullException("searchKeyExpression");
            if (searchPattern == null)
                throw new ArgumentNullException("searchPattern");
            if (!searchPattern.Any())
            {
                return searchSource.Take(0);
            }

            List<string> temp = new List<string>();
            foreach (var pattern in searchPattern)
            {
                if (!string.IsNullOrEmpty(pattern.Trim()))
                {
                    // I have changed this default logic, in order to sometimes we only need search the whole pattern.
                    // But, the default logic for String.Split():
                    // Although we give the parameter with: NULL, or Empty char array to String.Split()
                    // It'll split the source string with " ", which is not expected to us.(We expected search the whole pattern).
                    var tmp = seperators == null ? new string[] { pattern } : pattern.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Any())
                    {
                        foreach (var splitBySpaceCharacter in tmp)
                        {
                            if (!string.IsNullOrEmpty(splitBySpaceCharacter.Trim()))
                            {
                                temp.Add(splitBySpaceCharacter.Trim());
                            }
                        }
                    }
                }
            }
            if (!temp.Any())
            {
                return searchSource.Take(0);
            }
            else
            {
                searchPattern = temp;
            }

            var toLowerExpression = Expression.Call(searchKeyExpression.Body, typeof(string).GetMethod("ToLowerInvariant"));
            var notNullExpression = Expression.NotEqual(searchKeyExpression.Body, Expression.Constant(null));

            var expression = searchPattern.Select(v =>
                                                   Expression.Equal(Expression.Call(toLowerExpression, typeof(string).GetMethod("Contains"), Expression.Constant(v.ToLowerInvariant())),
                                                                    Expression.Constant(true)));

            var expressionAccumulation = expression.Aggregate((acc, exp) => Expression.OrElse(acc, exp));
            expressionAccumulation = Expression.AndAlso(notNullExpression, expressionAccumulation);

            return searchSource.Where(Expression.Lambda<Func<TEntity, bool>>(expressionAccumulation, searchKeyExpression.Parameters.Single()).Compile());
        }

        public static IEnumerable<TEntity> Random<TEntity>(this IEnumerable<TEntity> originEntities, int? randomSeed = null)
        {
            if (originEntities == null)
            {
                throw new ArgumentNullException("originEntities is null");
            }

            if (randomSeed == null)
            {
                randomSeed = DateTime.Now.Millisecond;
            }

            Random random = new Random(randomSeed.Value);
            var entities = originEntities.ToList();
            var totalCount = entities.Count;

            for (var i = 0; i < totalCount; i++)
            {
                int index = random.Next(0, totalCount);
                entities.Add(entities[index]);
                entities.RemoveAt(index);
            }

            return entities.AsEnumerable();
        }
        #endregion
    }

    #region Expression Utility
    public class ParameterRebinder : ExpressionVisitor
    {

        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }

    }

    public static class Utility
    {

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
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

        public static Expression<T> Or<T>(this Expression<T> first, Expression<T> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
    #endregion


}
