using FlashcardGen.Models.DbModels;

namespace FlashcardGen.Models
{
    public class OpenGNTRowEntities
    {
        public Lexeme Lexeme { get; set; }
        public WordForm WordForm { get; set; }
        public Verse Verse { get; set; }
        public WordFormOccurrence WordFormOccurrence { get; set; }
    }
}
