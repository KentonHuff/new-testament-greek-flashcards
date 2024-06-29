using FlashcardGen.Models;

namespace FlashcardGen.Common
{
    public static class Parsing
    {
        public static OpenGNTRowEntities ParseOpenGNTRow(string openGNTRow)
        {
            openGNTRow = openGNTRow.Replace("〕", "");

            string[] firstSplit = openGNTRow.Split("\t〔");

            string[] verseNumbers = firstSplit[2].Split('｜');

            OpenGNTRowEntities result = new();
            result.Verse = new()
            {
                BookNumber = Int32.Parse(verseNumbers[0]),
                ChapterNumber = Int32.Parse(verseNumbers[1]),
                VerseNumber = Int32.Parse(verseNumbers[2]),
            };

            string[] greekForms = firstSplit[3].Split('｜');
            string[] englishForms = firstSplit[6].Split('｜');
            string[] punctuationMarks = firstSplit[7].Split('｜');

            result.Lexeme = new()
            {
                ExtendedStrongsNumber = greekForms[5],
                TyndaleHouseGloss = englishForms[0],
                LexicalForm = greekForms[3],
            };

            result.WordForm = new()
            {
                LowercaseSpelling = greekForms[2].ToLowerInvariant(),
                RobinsonsMorphologicalAnalysisCode = greekForms[4],
            };

            result.WordFormOccurrence = new()
            {
                IsCapitalized = char.IsUpper(greekForms[2][0]),
                IsAllCaps = greekForms[2].Length > 1 && char.IsUpper(greekForms[2][1]),
                StudyTranslationGloss = englishForms[3],
                PreOccurrencePunctuationMarks = punctuationMarks[0].Replace("<pm>", "").Replace("</pm>", ""),
                PostOccurrencePunctuationMarks = punctuationMarks[1].Replace("<pm>", "").Replace("</pm>", ""),
            };

            return result;
        }
    }
}
