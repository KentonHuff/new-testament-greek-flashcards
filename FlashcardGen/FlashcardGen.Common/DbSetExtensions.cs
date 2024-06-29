using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace FlashcardGen.Common
{
    public static class DbSetExtensions
    {
        public static async Task<T> AddIfNotExistsAsync<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate) where T : class, new()
        {
            T? existingEntity = await dbSet.Where(predicate).SingleOrDefaultAsync();
            if (existingEntity is not null)
                return existingEntity;

            await dbSet.AddAsync(entity);
            return entity;
        }
    }
}
