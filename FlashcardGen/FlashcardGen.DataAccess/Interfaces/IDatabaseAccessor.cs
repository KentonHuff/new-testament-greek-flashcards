using FlashcardGen.Models.DbModels;

namespace FlashcardGen.DataAccess
{
    public interface IDatabaseAccessor
    {
        void LoadDb();
        IQueryable<WordForm> GetOrderedWordForms();
        WordFormOccurrence GetVerseForWordForm(WordForm wordForm);
        void AddCard(WordFormOccurrence verseForCard);
        void WriteDbToDisk();
        bool ShouldPopulateCardsTable();
        IQueryable<Card> GetCards();
    }
}
