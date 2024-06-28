namespace FlashcardGen.Models.DbModels
{
    public class Verse
    {
        public int VerseId { get; set; }

        public int BookNumber { get; set; }
        public int ChapterNumber { get; set; }
        public int VerseNumber { get; set; }

        public virtual ICollection<WordFormOccurrence> WordFormOccurrences { get; set; }
    }
}
