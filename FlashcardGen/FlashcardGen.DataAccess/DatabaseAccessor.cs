namespace FlashcardGen.DataAccess
{
    public class DatabaseAccessor : IDatabaseAccessor
    {
        private readonly OpenGreekNewTestamentContext _dbContext;

        public DatabaseAccessor(OpenGreekNewTestamentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async void Test()
        {
            await _dbContext.Database.EnsureCreatedAsync();

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
            Console.WriteLine(_dbContext.Verses.OrderBy(v => v.VerseId).Last().VerseNumber);
        }
    }
}
