using FlashcardGen.Common;
using FlashcardGen.Models;

namespace FlashcardGen.DataAccess
{
    public class DatabaseAccessor : IDatabaseAccessor
    {
        private readonly OpenGreekNewTestamentContext _dbContext;
        private readonly ILocalFileAccessor _localFileAccessor;

        public DatabaseAccessor(OpenGreekNewTestamentContext dbContext, ILocalFileAccessor localFileAccessor)
        {
            _dbContext = dbContext;
            _localFileAccessor = localFileAccessor;
        }

        public async Task LoadDb()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            /*string? currentOpenGNTRow = _localFileAccessor.GetNextOpenGNTRow();

            while (currentOpenGNTRow != null)
            {
                await AddEntitiesFromRow(currentOpenGNTRow);

                currentOpenGNTRow = _localFileAccessor.GetNextOpenGNTRow();
            }*/

            Console.WriteLine(_dbContext.Lexemes.Count());
            Console.WriteLine(_dbContext.WordForms.Count());
            Console.WriteLine(_dbContext.WordFormOccurrences.Count());
            Console.WriteLine(_dbContext.Verses.Count());
        }

        private async Task AddEntitiesFromRow(string openGNTRow)
        {
            OpenGNTRowEntities entities = Parsing.ParseOpenGNTRow(openGNTRow);

            entities.Lexeme = await _dbContext.Lexemes.AddIfNotExistsAsync(
                entity: entities.Lexeme,
                predicate: l => l.ExtendedStrongsNumber == entities.Lexeme.ExtendedStrongsNumber
                );
            //await _dbContext.SaveChangesAsync();
            entities.WordForm.LexemeId = entities.Lexeme.LexemeId;
            entities.WordForm.Lexeme = entities.Lexeme;

            entities.WordForm = await _dbContext.WordForms.AddIfNotExistsAsync(
                entity: entities.WordForm,
                predicate: wf => wf.LexemeId == entities.WordForm.LexemeId
                    && wf.LowercaseSpelling == entities.WordForm.LowercaseSpelling
                    && wf.RobinsonsMorphologicalAnalysisCode == entities.WordForm.RobinsonsMorphologicalAnalysisCode
            );
            //await _dbContext.SaveChangesAsync();
            entities.WordFormOccurrence.WordFormId = entities.WordForm.WordFormId;
            entities.WordFormOccurrence.WordForm = entities.WordForm;

            entities.Verse = await _dbContext.Verses.AddIfNotExistsAsync(
                entity: entities.Verse,
                predicate: v => v.BookNumber == entities.Verse.BookNumber
                    && v.ChapterNumber == entities.Verse.ChapterNumber
                    && v.VerseNumber == entities.Verse.VerseNumber
            );
            //await _dbContext.SaveChangesAsync();
            entities.WordFormOccurrence.VerseId = entities.Verse.VerseId;
            entities.WordFormOccurrence.Verse = entities.Verse;

            await _dbContext.WordFormOccurrences.AddAsync(entities.WordFormOccurrence);
            await _dbContext.SaveChangesAsync();
        }
    }
}
