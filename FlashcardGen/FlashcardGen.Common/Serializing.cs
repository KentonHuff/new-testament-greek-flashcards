using FlashcardGen.Models.DbModels;

namespace FlashcardGen.Common
{
    public static class Serializing
    {
        public static string SerializeCard(Card card)
        {
            return SerializeGreekVerse(card.WordFormOccurrence)+'\t'
                +SerializeEnglishVerse(card.WordFormOccurrence)+'\t'
                +card.WordFormOccurrence.WordForm.RobinsonsMorphologicalAnalysisCode+'\t'
                +"{{c1::"+card.WordFormOccurrence.WordForm.Lexeme.LexicalForm+"}}\t"
                +card.WordFormOccurrence.WordForm.Lexeme.TyndaleHouseGloss+'\t'
                +string.Join(", ", card.WordFormOccurrence.WordForm.WordFormOccurrences
                    .GroupBy(o => o.StudyTranslationGloss)
                    .Select(g => new
                    {
                        Gloss = g.Key,
                        Count = g.Count(),
                    })
                    .OrderByDescending(e => e.Count)
                    .Select(e => e.Gloss).Take(10))+'\t'
                +SerializeCitation(card.WordFormOccurrence.Verse)+'\t'
                +card.WordFormOccurrence.WordForm.Lexeme.ExtendedStrongsNumber;
        }

        private static string SerializeGreekVerse(WordFormOccurrence occurrence)
        {
            string result = "";

            foreach (WordFormOccurrence currentOccurrence in occurrence.Verse.WordFormOccurrences.OrderBy(o => o.WordFormOccurrenceId))
            {
                foreach (char prePunctuationMark in currentOccurrence.PreOccurrencePunctuationMarks)
                {
                    if (!Constants.GreekPreOccurrencePunctuationMapping.ContainsKey(prePunctuationMark))
                        throw new KeyNotFoundException($"Greek pre-occurrence mark {prePunctuationMark} not found in mapping.");
                    
                    result += Constants.GreekPreOccurrencePunctuationMapping[prePunctuationMark];
                }

                if (currentOccurrence.WordFormOccurrenceId == occurrence.WordFormOccurrenceId)
                    result += "<span style=\"color:blue\">{{c1::";

                string spelling = currentOccurrence.WordForm.LowercaseSpelling;

                if (currentOccurrence.IsAllCaps)
                    spelling = spelling.ToUpperInvariant();
                else if (currentOccurrence.IsCapitalized)
                    spelling = char.ToUpperInvariant(spelling[0]) + spelling.Substring(1);

                result += spelling;

                if (currentOccurrence.WordFormOccurrenceId == occurrence.WordFormOccurrenceId)
                    result += "}}</span>";

                foreach (char postPunctuationMark in currentOccurrence.PostOccurrencePunctuationMarks)
                {
                    if (!Constants.GreekPostOccurrencePunctuationMapping.ContainsKey(postPunctuationMark))
                        throw new KeyNotFoundException($"Greek post-occurrence mark {postPunctuationMark} not found in mapping.");

                    result += Constants.GreekPostOccurrencePunctuationMapping[postPunctuationMark];
                }

                if (currentOccurrence.PostOccurrencePunctuationMarks.Length == 0)
                    result += " ";
            }

            return result;
        }

        private static string SerializeEnglishVerse(WordFormOccurrence occurrence)
        {
            string result = "";

            foreach (WordFormOccurrence currentOccurrence in occurrence.Verse.WordFormOccurrences.OrderBy(o => o.WordFormOccurrenceId))
            {
                foreach (char prePunctuationMark in currentOccurrence.PreOccurrencePunctuationMarks)
                {
                    if (!Constants.EnglishPreOccurrencePunctuationMapping.ContainsKey(prePunctuationMark))
                        throw new KeyNotFoundException($"English pre-occurrence mark {prePunctuationMark} not found in mapping.");

                    result += Constants.EnglishPreOccurrencePunctuationMapping[prePunctuationMark];
                }

                if (currentOccurrence.WordFormOccurrenceId == occurrence.WordFormOccurrenceId)
                    result += "<span style=\"color:blue\">";

                result += currentOccurrence.StudyTranslationGloss;

                if (currentOccurrence.WordFormOccurrenceId == occurrence.WordFormOccurrenceId)
                    result += "</span>";

                foreach (char postPunctuationMark in currentOccurrence.PostOccurrencePunctuationMarks)
                {
                    if (!Constants.EnglishPostOccurrencePunctuationMapping.ContainsKey(postPunctuationMark))
                        throw new KeyNotFoundException($"English post-occurrence mark {postPunctuationMark} not found in mapping.");

                    result += Constants.EnglishPostOccurrencePunctuationMapping[postPunctuationMark];
                }

                if (currentOccurrence.PostOccurrencePunctuationMarks.Length == 0)
                    result += " ";
            }

            return result;
        }

        private static string SerializeCitation(Verse verse)
        {
            return $"{Constants.BookNumberToName[verse.BookNumber]} {verse.ChapterNumber}:{verse.VerseNumber}";
        }
    }
}
