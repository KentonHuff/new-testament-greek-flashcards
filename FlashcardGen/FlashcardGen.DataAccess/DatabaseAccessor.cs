using FlashcardGen.Common;
using FlashcardGen.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace FlashcardGen.DataAccess
{
    public class DatabaseAccessor : IDatabaseAccessor
    {
        private readonly OpenGreekNewTestamentContext _dbContext;
        private readonly ILocalFileAccessor _localFileAccessor;
        private readonly IConfiguration _configuration;

        public DatabaseAccessor(OpenGreekNewTestamentContext dbContext, ILocalFileAccessor localFileAccessor, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _localFileAccessor = localFileAccessor;
            _configuration = configuration;
        }

        public async Task LoadDb()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            if (bool.Parse(_configuration[Constants.ConfigPaths.ReadDbFromDisk]!) && File.Exists(Constants.LocalFiles.SQLiteDb))
            {
                //We already read the db from disk in Program.cs
                return;
            }
            else if (bool.Parse(_configuration[Constants.ConfigPaths.ReadUncompressedOpenGNTBaseText]!) && File.Exists(Constants.LocalFiles.InputFilesPath + Constants.LocalFiles.OpenGreekNewTestamentFileName))
            {
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
            }
            else
            {
                throw new NotImplementedException();
            }

            _dbContext.WordForms.Select(wf => wf.RobinsonsMorphologicalAnalysisCode).Distinct().OrderBy(x => x).Take(1).ToList().ForEach(f => Console.WriteLine(f));
            //_dbContext.WordForms.Where(wf => wf.LowercaseSpelling.Length < 1).Select(wf => wf.LowercaseSpelling).ToList().ForEach(f => Console.WriteLine(f));
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
