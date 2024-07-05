using FlashcardGen.Models.DbModels;

namespace FlashcardGen.Common
{
    public static class Serializing
    {
        public static string SerializeCard(Card card)
        {
            return SerializeGreekVerse(card.WordFormOccurrence);
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
    }
}
