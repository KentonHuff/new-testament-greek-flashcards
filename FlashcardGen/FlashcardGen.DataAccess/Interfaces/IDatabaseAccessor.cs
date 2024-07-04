using FlashcardGen.Models.DbModels;

namespace FlashcardGen.DataAccess
{
    public interface IDatabaseAccessor
    {
        public void LoadDb();
        public IQueryable<WordForm> GetOrderedWordForms();
        public WordFormOccurrence GetVerseForWordForm(WordForm wordForm);
    }
}
