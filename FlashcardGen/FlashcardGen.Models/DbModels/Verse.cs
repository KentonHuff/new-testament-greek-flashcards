using Microsoft.EntityFrameworkCore;

namespace FlashcardGen.Models.DbModels
{
    [Index(nameof(BookNumber), nameof(ChapterNumber), nameof(VerseNumber))]
    public class Verse
    {
        public int VerseId { get; set; }

        public int BookNumber { get; set; }
        public int ChapterNumber { get; set; }
        public int VerseNumber { get; set; }

        public virtual ICollection<WordFormOccurrence> WordFormOccurrences { get; set; }
    }
}
