namespace FlashcardGen.DataAccess
{
    public interface ILocalFileAccessor
    {
        string? GetNextOpenGNTRow();
        void WriteFlashcard(string flashcard);
    }
}
