using System.Diagnostics;
using System.Linq.Expressions;
using FlashcardGen.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace FlashcardGen.Common
{
    public static class DbSetExtensions
    {
        public static HashSet<string> issues = new HashSet<string>();
        public static HashSet<string> grammaticalForms = new HashSet<string>();

        public static async Task<T> AddIfNotExistsAsync<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate) where T : class, new()
        {
            if ((entity as WordForm).LowercaseSpelling.Length < 1)
                issues.Add($"[WordForm] Spelling is empty string");

            T? existingEntity = await dbSet.Where(predicate).SingleOrDefaultAsync();
            if (existingEntity is not null)
            {
                TestProperties(entity, existingEntity);
                return existingEntity;
            }

            await dbSet.AddAsync(entity);
            return entity;
        }

        private static void TestProperties<T>(T entity, T existingEntity) where T : class, new()
        {
            switch (entity)
            {
                case Lexeme entityLexeme:
                    if (entityLexeme.ExtendedStrongsNumber != (existingEntity as Lexeme).ExtendedStrongsNumber)
                        issues.Add($"[Lexeme] Strong's: {entityLexeme.ExtendedStrongsNumber}, {(existingEntity as Lexeme).ExtendedStrongsNumber}");
                    break;
                case WordForm entityWordForm:
                    if (entityWordForm.LowercaseSpelling.Length < 1)
                        issues.Add($"[WordForm] Spelling is empty string");
                    break;
                case Verse entityVerse:
                    break;
            }
        }
    }
}
