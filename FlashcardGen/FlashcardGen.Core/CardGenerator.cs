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

        public async Task GenerateCards()
        {
            Console.WriteLine("Generating cards...");
            Console.WriteLine(_configuration[Constants.ConfigPaths.OpenGNTBaseTextZipURL]);
            await _databaseAccessor.LoadDb();

            IQueryable<WordForm> wordForms = _databaseAccessor.GetOrderedWordForms();
            foreach (var wordForm in wordForms.Take(1))
            {
                Console.WriteLine($"{wordForm.Lexeme.ExtendedStrongsNumber}\t{wordForm.Lexeme.LexicalForm}\t{wordForm.RobinsonsMorphologicalAnalysisCode}\t{wordForm.LowercaseSpelling}");
            }
        }
    }
}
