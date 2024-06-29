using FlashcardGen.Models.DbModels;

namespace FlashcardGen.Models
{
    public class DataRowEntities
    {
        public Lexeme Lexeme { get; set; }
        public WordForm WordForm { get; set; }
        public Verse Verse { get; set; }
        public WordFormOccurrence WordFormOccurrence { get; set; }
    }
}
