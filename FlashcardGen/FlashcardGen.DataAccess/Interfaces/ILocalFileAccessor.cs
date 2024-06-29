using FlashcardGen.Models;

namespace FlashcardGen.DataAccess
{
    public interface ILocalFileAccessor
    {
        DataRowEntities ProcessNextRow();
    }
}
