namespace FlashcardGen.DataAccess
{
    public interface ILocalFileAccessor
    {
        string? GetNextOpenGNTRow();
    }
}
