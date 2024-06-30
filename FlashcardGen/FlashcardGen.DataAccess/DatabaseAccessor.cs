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
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            string? currentOpenGNTRow = _localFileAccessor.GetNextOpenGNTRow();

            int currentOccurrenceNumber = 0;

            while (currentOpenGNTRow != null)
            {
                if (currentOccurrenceNumber % 1000 == 0)
                    Console.WriteLine($"Populating database: {Math.Round(100 * (double)currentOccurrenceNumber / Constants.NumberOfWordOccurrences, 1)}%");

                await AddEntitiesFromRow(currentOpenGNTRow);

                ++currentOccurrenceNumber;

                currentOpenGNTRow = _localFileAccessor.GetNextOpenGNTRow();
            }

            _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            Console.WriteLine(_dbContext.Lexemes.Count());
            Console.WriteLine(_dbContext.WordForms.Count());
            Console.WriteLine(_dbContext.WordFormOccurrences.Count());
            Console.WriteLine(_dbContext.Verses.Count());

            _dbContext.WordForms.Select(wf => wf.RobinsonsMorphologicalAnalysisCode).Distinct().OrderBy(x => x).ToList().ForEach(f => Console.WriteLine(f));
        }

        private async Task AddEntitiesFromRow(string openGNTRow)
        {
            OpenGNTRowEntities entities = Parsing.ParseOpenGNTRow(openGNTRow);

            entities.Lexeme = await _dbContext.Lexemes.AddIfNotExistsAsync(
                entity: entities.Lexeme,
                predicate: l => l.LexicalForm == entities.Lexeme.LexicalForm
                    && l.TyndaleHouseGloss == entities.Lexeme.TyndaleHouseGloss
                );
            entities.WordForm.LexemeId = entities.Lexeme.LexemeId;
            entities.WordForm.Lexeme = entities.Lexeme;

            entities.WordForm = await _dbContext.WordForms.AddIfNotExistsAsync(
                entity: entities.WordForm,
                predicate: wf => wf.LowercaseSpelling == entities.WordForm.LowercaseSpelling
                    && wf.RobinsonsMorphologicalAnalysisCode == entities.WordForm.RobinsonsMorphologicalAnalysisCode
                    && wf.LexemeId == entities.WordForm.LexemeId
            );
            entities.WordFormOccurrence.WordFormId = entities.WordForm.WordFormId;
            entities.WordFormOccurrence.WordForm = entities.WordForm;

            entities.Verse = await _dbContext.Verses.AddIfNotExistsAsync(
                entity: entities.Verse,
                predicate: v => v.BookNumber == entities.Verse.BookNumber
                    && v.ChapterNumber == entities.Verse.ChapterNumber
                    && v.VerseNumber == entities.Verse.VerseNumber
            );
            entities.WordFormOccurrence.VerseId = entities.Verse.VerseId;
            entities.WordFormOccurrence.Verse = entities.Verse;

            await _dbContext.WordFormOccurrences.AddAsync(entities.WordFormOccurrence);
            await _dbContext.SaveChangesAsync();
        }
    }
}
