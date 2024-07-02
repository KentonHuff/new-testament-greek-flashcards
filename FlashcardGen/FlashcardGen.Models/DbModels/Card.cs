namespace FlashcardGen.Models.DbModels
{
    public class Card
    {
        public int CardId { get; set; }

        public int WordFormOccurrenceId { get; set; }
        public virtual WordFormOccurrence WordFormOccurrence { get; set; }
    }
}
