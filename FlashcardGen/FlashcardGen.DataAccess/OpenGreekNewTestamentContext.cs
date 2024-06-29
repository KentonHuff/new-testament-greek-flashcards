using Microsoft.EntityFrameworkCore;
using FlashcardGen.Models.DbModels;

namespace FlashcardGen.DataAccess;
public class OpenGreekNewTestamentContext : DbContext
{
    public DbSet<Lexeme> Lexemes { get; set; }
    public DbSet<WordForm> WordForms { get; set; }
    public DbSet<Verse> Verses { get; set; }
    public DbSet<WordFormOccurrence> WordFormOccurrences { get; set; }

    public OpenGreekNewTestamentContext()
    {
    }

    public OpenGreekNewTestamentContext(DbContextOptions<OpenGreekNewTestamentContext> options) : base(options)
    {
    }
}
