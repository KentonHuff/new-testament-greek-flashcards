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

        public async void Test()
        {
            Console.WriteLine(_localFileAccessor.GetNextOpenGNTRow());
            /*await _dbContext.Database.EnsureCreatedAsync();

            _dbContext.Verses.Add(new Models.DbModels.Verse
            {
                BookNumber = 40,
                ChapterNumber = 1,
                VerseNumber = 1,
            });

            _dbContext.Verses.Add(new Models.DbModels.Verse
            {
                BookNumber = 40,
                ChapterNumber = 1,
                VerseNumber = 2,
            });

            _dbContext.SaveChanges();

            Console.WriteLine(_dbContext.Verses.OrderBy(v => v.VerseId).Last().VerseId);
            Console.WriteLine(_dbContext.Verses.OrderBy(v => v.VerseId).Last().VerseNumber);*/
        }
    }
}
