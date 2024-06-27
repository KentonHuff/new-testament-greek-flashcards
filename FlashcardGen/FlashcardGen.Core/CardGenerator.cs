using FlashcardGen.Common;
using Microsoft.Extensions.Configuration;

namespace FlashcardGen.Core
{
    public class CardGenerator : ICardGenerator
    {
        private readonly IConfiguration _configuration;

        public CardGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void GenerateCards()
        {
            Console.WriteLine("Generating cards...");
            Console.WriteLine(_configuration[Constants.ConfigPaths.OpenGNTBaseTextZipURL]);
        }
    }
}
