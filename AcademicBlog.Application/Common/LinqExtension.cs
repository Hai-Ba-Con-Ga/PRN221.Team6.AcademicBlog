
using System.Linq.Expressions;


namespace AcademicBlog.Application.Common
{
    public static class LinqExtension
    {
        public static IQueryable<T> Includes<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            foreach (var include in includes)
            {
                query = query.Includes(include);
            }
            return query;
        }
    }
}
