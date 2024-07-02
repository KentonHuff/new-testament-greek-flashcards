using FlashcardGen.Common;
using FlashcardGen.Models;
using FlashcardGen.Models.DbModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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

            if (_dbContext.Lexemes.Any())
            {
                //We already read the db from disk in Program.cs
                return;
            }
            else if (bool.Parse(_configuration[Constants.ConfigPaths.ReadUncompressedOpenGNTBaseText]!))
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                string? currentOpenGNTRow = _localFileAccessor.GetNextOpenGNTRow();

                for (int i = 0; currentOpenGNTRow != null; ++i)
                {
                    if (i % 1000 == 0)
                        Console.WriteLine($"Populating database: {Math.Round(100 * (double)i / Constants.NumberOfWordOccurrences, 1)}%");

                    await AddEntitiesFromRow(currentOpenGNTRow);

                    currentOpenGNTRow = _localFileAccessor.GetNextOpenGNTRow();
                }

                _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IQueryable<WordForm> GetOrderedWordForms()
        {
            var result = (
                from wordForm in _dbContext.WordForms
                    join lexeme in (from occurrence in _dbContext.WordFormOccurrences
                            group occurrence by occurrence.WordForm.LexemeId into lexemeGroup
                            select new { LexemeId = lexemeGroup.Key, NumOccurrences = lexemeGroup.Count() })
                        on wordForm.LexemeId equals lexeme.LexemeId
                orderby lexeme.NumOccurrences descending, lexeme.LexemeId, wordForm.RobinsonsMorphologicalAnalysisCode
                select wordForm
            ).Include(wf => wf.Lexeme);
            return result;
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
