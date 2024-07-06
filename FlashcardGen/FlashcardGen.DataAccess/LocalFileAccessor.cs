using FlashcardGen.Common;
using System.Text;

namespace FlashcardGen.DataAccess
{
    public class LocalFileAccessor : ILocalFileAccessor
    {
        private IEnumerator<string> _openGNTLines;
        private StreamWriter _cardWriter;
        public LocalFileAccessor()
        {
            _openGNTLines = File.ReadLines(
                path: Constants.LocalFiles.InputFilesPath + Constants.LocalFiles.OpenGreekNewTestamentFileName,
                encoding: Encoding.UTF8
            ).GetEnumerator();

            _ = _openGNTLines.MoveNext(); //Skip the column name row

            _cardWriter = new StreamWriter(Constants.LocalFiles.OutputFilePath, false);
            _cardWriter.AutoFlush = true;
        }

        public void WriteFlashcard(string flashcard)
        {
            _cardWriter.WriteLine(flashcard);
        }

        public string? GetNextOpenGNTRow()
        {
            if (_openGNTLines is null)
                return null;

            _ = _openGNTLines.MoveNext();

            if (_openGNTLines.Current?.Length > 0)
                return _openGNTLines.Current;

            _openGNTLines.Dispose();
            return null;
        }
    }
}
