using FlashcardGen.Common;
using System.Text;

namespace FlashcardGen.DataAccess
{
    public class LocalFileAccessor : ILocalFileAccessor
    {
        private IEnumerator<string> _openGNTLines;
        public LocalFileAccessor()
        {
            _openGNTLines = File.ReadLines(
                path: Constants.LocalFiles.OpenGreekNewTestamentPath + Constants.LocalFiles.OpenGreekNewTestamentFileName,
                encoding: Encoding.UTF8
            ).GetEnumerator();

            _ = _openGNTLines.MoveNext(); //Skip the column name row
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
