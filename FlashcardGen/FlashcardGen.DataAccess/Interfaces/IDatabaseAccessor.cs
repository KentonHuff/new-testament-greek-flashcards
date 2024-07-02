using FlashcardGen.Models.DbModels;

namespace FlashcardGen.DataAccess
{
    public interface IDatabaseAccessor
    {
        public Task LoadDb();
        public IQueryable<WordForm> GetOrderedWordForms();
    }
}
