using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace FlashcardGen.Common
{
    public static class DbSetExtensions
    {
        public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate) where T : class, new()
        {
            T? existingEntity = dbSet.Where(predicate).SingleOrDefault();
            if (existingEntity is not null)
                return existingEntity;

            dbSet.Add(entity);
            return entity;
        }
    }
}
