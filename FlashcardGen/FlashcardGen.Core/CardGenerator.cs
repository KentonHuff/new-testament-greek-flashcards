using FlashcardGen.Common;
using Microsoft.Extensions.Configuration;
using FlashcardGen.DataAccess;
using FlashcardGen.Models.DbModels;

namespace FlashcardGen.Core
{
    public class CardGenerator : ICardGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseAccessor _databaseAccessor;

        public CardGenerator(IConfiguration configuration, IDatabaseAccessor databaseAccessor)
        {
            _configuration = configuration;
            _databaseAccessor = databaseAccessor;
        }

        public void GenerateCards()
        {
            Console.WriteLine("Generating cards...");
            Console.WriteLine(_configuration[Constants.ConfigPaths.OpenGNTBaseTextZipURL]);
            _databaseAccessor.LoadDb();

            IQueryable<WordForm> wordForms = _databaseAccessor.GetOrderedWordForms();

            int i = 0;
            foreach (var wordForm in wordForms)
            {
                var verseForWordFrom = _databaseAccessor.GetVerseForWordForm(wordForm);
                if (i % 500 == 0)
                    Console.WriteLine(++i);
                _databaseAccessor.AddCard(verseForWordFrom);
            }
        }
    }
}
