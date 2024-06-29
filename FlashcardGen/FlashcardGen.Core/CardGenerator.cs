using FlashcardGen.Common;
using Microsoft.Extensions.Configuration;
using FlashcardGen.DataAccess;

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
            _databaseAccessor.Test();
        }
    }
}
