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
        private readonly SqliteConnection _inMemoryConnection;

        public DatabaseAccessor(
            OpenGreekNewTestamentContext dbContext,
            ILocalFileAccessor localFileAccessor,
            IConfiguration configuration,
            SqliteConnection inMemoryConnection
        )
        {
            _dbContext = dbContext;
            _localFileAccessor = localFileAccessor;
            _configuration = configuration;
            _inMemoryConnection = inMemoryConnection;
        }

        public void LoadDb()
        {
            _dbContext.Database.EnsureCreated();
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            if (bool.Parse(_configuration[Constants.ConfigPaths.ReadDbFromDisk]!) && File.Exists(Constants.LocalFiles.SQLiteDb))
            {
                using (var onDiskConnection = new SqliteConnection(Constants.ConnectionStrings.OnDisk))
                {
                    onDiskConnection.Open();
                    onDiskConnection.BackupDatabase(_inMemoryConnection);
                }
            }
            else if (bool.Parse(_configuration[Constants.ConfigPaths.ReadUncompressedOpenGNTBaseText]!))
            {
                string? currentOpenGNTRow = _localFileAccessor.GetNextOpenGNTRow();

                for (int i = 0; currentOpenGNTRow != null; ++i)
                {
                    if (i % 1000 == 0)
                        Console.WriteLine($"Populating database: {Math.Round(100 * (double)i / Constants.NumberOfWordOccurrences, 1)}%");

                    AddEntitiesFromRow(currentOpenGNTRow);

                    currentOpenGNTRow = _localFileAccessor.GetNextOpenGNTRow();
                }

                //_dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
            else
            {
                throw new NotImplementedException();
            }

            Console.WriteLine(_dbContext.Lexemes.Count());
            Console.WriteLine(_dbContext.WordForms.Count());
            Console.WriteLine(_dbContext.WordFormOccurrences.Count());
            Console.WriteLine(_dbContext.Verses.Count());
            Console.WriteLine(_dbContext.Cards.Count());
        }

        public void WriteDbToDisk()
        {
            if (bool.Parse(_configuration[Constants.ConfigPaths.WriteDbToDisk]!))
            {
                using (var onDiskConnection = new SqliteConnection(Constants.ConnectionStrings.OnDisk))
                {
                    onDiskConnection.Open();
                    _inMemoryConnection.BackupDatabase(onDiskConnection);
                }
            }
        }

        public bool ShouldPopulateCardsTable()
        {
            return _dbContext.Cards.Count() == 0;
        }

        public IQueryable<Card> GetCards()
        {
            return _dbContext.Cards
                //.AsNoTracking()
                .OrderBy(c => c.CardId)
                .Include(c => c.WordFormOccurrence)
                    .ThenInclude(wfo => wfo.WordForm)
                        .ThenInclude(wf => wf.Lexeme)
                .Include(c => c.WordFormOccurrence)
                    .ThenInclude(wfo => wfo.Verse)
                        .ThenInclude(v => v.WordFormOccurrences)
                            .ThenInclude(wfo => wfo.WordForm)
                 .Include(c => c.WordFormOccurrence)
                    .ThenInclude(wfo => wfo.WordForm)
                        .ThenInclude(wf => wf.WordFormOccurrences);
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
            ).AsNoTracking();
            return result;
        }

        public WordFormOccurrence GetVerseForWordForm(WordForm wordForm)
        {
            var result = _dbContext.WordFormOccurrences
                .AsNoTracking()
                .Where(wfo => wfo.WordFormId == wordForm.WordFormId)
                .Select(wfo => new
                {
                    WordFormOccurrence = wfo,
                    VersesNumUnknownOccurrences = wfo.Verse.WordFormOccurrences
                        .Where(wfo => !_dbContext.Cards.Select(c => c.WordFormOccurrence.WordFormId).Contains(wfo.WordFormId)).Count(),
                    VersesNumWords = wfo.Verse.WordFormOccurrences.Count(),
                    VersesNumCards = _dbContext.Cards.Where(c => c.WordFormOccurrence.VerseId == wfo.VerseId).Count(),
                })
                .OrderBy(e => e.VersesNumUnknownOccurrences)
                .ThenBy(e => e.VersesNumCards)
                .Select(e => e.WordFormOccurrence)
                //.Include(wfo => wfo.Verse)
                //    .ThenInclude(v => v.WordFormOccurrences)
                //        .ThenInclude(wfo => wfo.WordForm)
                .First();
            return result;
        }

        public void AddCard(WordFormOccurrence verseForCard)
        {
            _dbContext.Cards.Add(new Card
            {
                WordFormOccurrenceId = verseForCard.WordFormOccurrenceId,
            });

            _dbContext.SaveChanges();
        }

        private void AddEntitiesFromRow(string openGNTRow)
        {
            OpenGNTRowEntities entities = Parsing.ParseOpenGNTRow(openGNTRow);

            entities.Lexeme = _dbContext.Lexemes.AddIfNotExists(
                entity: entities.Lexeme,
                predicate: l => l.LexicalForm == entities.Lexeme.LexicalForm
                    && l.TyndaleHouseGloss == entities.Lexeme.TyndaleHouseGloss
                );
            entities.WordForm.LexemeId = entities.Lexeme.LexemeId;
            entities.WordForm.Lexeme = entities.Lexeme;

            entities.WordForm = _dbContext.WordForms.AddIfNotExists(
                entity: entities.WordForm,
                predicate: wf => wf.LowercaseSpelling == entities.WordForm.LowercaseSpelling
                    && wf.RobinsonsMorphologicalAnalysisCode == entities.WordForm.RobinsonsMorphologicalAnalysisCode
                    && wf.LexemeId == entities.WordForm.LexemeId
            );
            entities.WordFormOccurrence.WordFormId = entities.WordForm.WordFormId;
            entities.WordFormOccurrence.WordForm = entities.WordForm;

            entities.Verse = _dbContext.Verses.AddIfNotExists(
                entity: entities.Verse,
                predicate: v => v.BookNumber == entities.Verse.BookNumber
                    && v.ChapterNumber == entities.Verse.ChapterNumber
                    && v.VerseNumber == entities.Verse.VerseNumber
            );
            entities.WordFormOccurrence.VerseId = entities.Verse.VerseId;
            entities.WordFormOccurrence.Verse = entities.Verse;

            _dbContext.WordFormOccurrences.Add(entities.WordFormOccurrence);
            _dbContext.SaveChanges();
        }
    }
}
