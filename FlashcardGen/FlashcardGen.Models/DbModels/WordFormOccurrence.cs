namespace FlashcardGen.Models.DbModels
{
    public class WordFormOccurrence
    {
        public int WordFormOccurrenceId { get; set; }

        public bool IsCapitalized { get; set; }
        //public int PositionInVerse { get; set; }
        public string StudyTranslationGloss { get; set; }
        public string PreOccurrencePunctuationMarks { get; set; }
        public string PostOccurrencePunctuationMarks { get; set; }

        public int WordFormId { get; set; }
        public virtual WordForm WordForm { get; set; }
        public int VerseId { get; set; }
        public virtual Verse Verse { get; set; }
    }
}
