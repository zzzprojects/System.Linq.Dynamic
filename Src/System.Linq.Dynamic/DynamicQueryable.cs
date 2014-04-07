using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using FluentValidation;

namespace System.Linq.Dynamic
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying data 
    /// structures that implement <see cref="IQueryable"/>. It allows dynamic string based querying. 
    /// Very handy when, at compile time, you don't know the type of queries that will be generated, 
    /// or when downstream components only return column names to sort and filter by.
    /// </summary>
    public static class DynamicQueryable
    {

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A <see cref="IQueryable{T}"/> to filter.</param>
        /// <param name="predicate">An expression string to test each element for a condition.</param>
        /// <param name="args">An object array that contains zero or more objects to insert into the predicate as parameters.  Similiar to the way String.Format formats strings.</param>
        /// <returns>A <see cref="IQueryable{T}"/> that contains elements from the input sequence that satisfy the condition specified by predicate.</returns>
        /// <example>
        /// <code>
        /// var result1 = list.Where("NumberProperty=1");
        /// var result2 = list.Where("NumberProperty=@0", 1);
        /// var result3 = list.Where("NumberProperty=@0", SomeIntValue);
        /// </code>
        /// </example>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, string predicate, params object[] args)
        {
            return (IQueryable<TSource>)Where((IQueryable)source, predicate, args);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="source">A <see cref="IQueryable"/> to filter.</param>
        /// <param name="predicate">An expression string to test each element for a condition.</param>
        /// <param name="args">An object array that contains zero or more objects to insert into the predicate as parameters.  Similiar to the way String.Format formats strings.</param>
        /// <returns>A <see cref="IQueryable"/> that contains elements from the input sequence that satisfy the condition specified by predicate.</returns>
        /// <example>
        /// <code>
        /// var result1 = list.Where("NumberProperty=1");
        /// var result2 = list.Where("NumberProperty=@0", 1);
        /// var result3 = list.Where("NumberProperty=@0", SomeIntValue);
        /// </code>
        /// </example>
        public static IQueryable Where(this IQueryable source, string predicate, params object[] args)
        {
            Validate.Argument(source, "source").IsNotNull().Check()
                    .Argument(predicate, "predicate").IsNotNull().IsNotEmpty().IsNotWhiteSpace().Check();

            LambdaExpression lambda = DynamicExpression.ParseLambda(source.ElementType, typeof(bool), predicate, args);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Where",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Quote(lambda)));
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A projection string to apply to each element.</param>
        /// <param name="args">An object array that contains zero or more objects to insert into the predicate as parameters.  Similiar to the way String.Format formats strings.</param>
        /// <returns>An <see cref="IQueryable{T}"/> whose elements are the result of invoking a projection string on each element of source.</returns>
        /// <example>
        /// <code>
        /// var singleField = qry.Select("StringProperty");
        /// var dynamicObject = qry.Select("new (StringProperty1, StringProperty2 as OtherStringPropertyName)");
        /// </code>
        /// </example>
        public static IQueryable Select(this IQueryable source, string selector, params object[] args)
        {
            Validate.Argument(source, "source").IsNotNull().Check()
                    .Argument(selector, "selector").IsNotNull().IsNotEmpty().IsNotWhiteSpace().Check();

            LambdaExpression lambda = DynamicExpression.ParseLambda(source.ElementType, null, selector, args);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Select",
                    new Type[] { source.ElementType, lambda.Body.Type },
                    source.Expression, Expression.Quote(lambda)));
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="ordering">An expression string to indicate values to order by.</param>
        /// <param name="args">An object array that contains zero or more objects to insert into the predicate as parameters.  Similiar to the way String.Format formats strings.</param>
        /// <returns>A <see cref="IQueryable{T}"/> whose elements are sorted according to the specified <paramref name="ordering"/>.</returns>
        /// <example>
        /// <code>
        /// var result = list.OrderBy("NumberProperty, StringProperty DESC");
        /// </code>
        /// </example>
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string ordering, params object[] args)
        {
            return (IQueryable<TSource>)OrderBy((IQueryable)source, ordering, args);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="ordering">An expression string to indicate values to order by.</param>
        /// <param name="args">An object array that contains zero or more objects to insert into the predicate as parameters.  Similiar to the way String.Format formats strings.</param>
        /// <returns>A <see cref="IQueryable"/> whose elements are sorted according to the specified <paramref name="ordering"/>.</returns>
        /// <example>
        /// <code>
        /// var result = list.OrderBy("NumberProperty, StringProperty DESC");
        /// </code>
        /// </example>
        public static IQueryable OrderBy(this IQueryable source, string ordering, params object[] args)
        {
            Validate.Argument(source, "source").IsNotNull().Check()
                    .Argument(ordering, "ordering").IsNotNull().IsNotEmpty().IsNotWhiteSpace().Check();


            ParameterExpression[] parameters = new ParameterExpression[] {
                Expression.Parameter(source.ElementType, "") };
            ExpressionParser parser = new ExpressionParser(parameters, ordering, args);
            IEnumerable<DynamicOrdering> orderings = parser.ParseOrdering();
            Expression queryExpr = source.Expression;
            string methodAsc = "OrderBy";
            string methodDesc = "OrderByDescending";
            foreach (DynamicOrdering o in orderings)
            {
                queryExpr = Expression.Call(
                    typeof(Queryable), o.Ascending ? methodAsc : methodDesc,
                    new Type[] { source.ElementType, o.Selector.Type },
                    queryExpr, Expression.Quote(Expression.Lambda(o.Selector, parameters)));
                methodAsc = "ThenBy";
                methodDesc = "ThenByDescending";
            }
            return source.Provider.CreateQuery(queryExpr);
        }


        /// <summary>
        /// Groups the elements of a sequence according to a specified key string function 
        /// and creates a result value from each group and its key.
        /// </summary>
        /// <param name="source">A <see cref="IQueryable"/> whose elements to group.</param>
        /// <param name="keySelector">A string to specify the key for each element.</param>
        /// <param name="resultSelector">A string to specify a result value from each group.</param>
        /// <param name="args">An object array that contains zero or more objects to insert into the predicate as parameters.  Similiar to the way String.Format formats strings.</param>
        /// <returns>A <see cref="IQueryable"/> where each element represents a projection over a group and its key.</returns>
        public static IQueryable GroupBy(this IQueryable source, string keySelector, string resultSelector, params object[] args)
        {
            Validate.Argument(source, "source").IsNotNull().Check()
                    .Argument(keySelector, "keySelector").IsNotNull().IsNotEmpty().IsNotWhiteSpace().Check()
                    .Argument(resultSelector, "resultSelector").IsNotNull().IsNotEmpty().IsNotWhiteSpace().Check();

            LambdaExpression keyLambda = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, args);
            LambdaExpression elementLambda = DynamicExpression.ParseLambda(source.ElementType, null, resultSelector, args);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "GroupBy",
                    new Type[] { source.ElementType, keyLambda.Body.Type, elementLambda.Body.Type },
                    source.Expression, Expression.Quote(keyLambda), Expression.Quote(elementLambda)));
        }

    }







}

