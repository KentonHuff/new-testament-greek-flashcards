using FlashcardGen.Models.DbModels;
using System.Runtime.CompilerServices;

namespace FlashcardGen.Common
{
    public static class Serializing
    {
        public static string SerializeCard(Card card)
        {
            return SerializeGreekVerse(card.WordFormOccurrence)+'\t'
                +SerializeEnglishVerse(card.WordFormOccurrence)+'\t'
                +SerializeRMAC(card.WordFormOccurrence.WordForm.RobinsonsMorphologicalAnalysisCode)+'\t'
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

        public static string Left (this string s, int length)
        {
            if (s.Length > length)
                return s.Substring(0, length);
            return s;
        }

        public static string Right(this string s, int length)
        {
            if (s.Length > length)
                return s.Substring(s.Length - length, length);
            return s;
        }

        private static string SerializeRMAC(string rmac)
        {
            string SerializeNounLikeForm(string form)
            {
                string result = "";

                int i = 0;

                if ((new HashSet<char> { '1', '2', '3' }).Contains(form[i]))
                {
                    switch (form[i])
                    {
                        case '1':
                            result += " 1st Person";
                            break;
                        case '2':
                            result += " 2nd Person";
                            break;
                        case '3':
                            result += " 3rd Person";
                            break;
                        default:
                            throw new KeyNotFoundException(form);
                    }
                    ++i;
                }
                if ((new HashSet<char> { 'S', 'P' }).Contains(form[i]))
                {
                    switch (form[i])
                    {
                        case 'S':
                            result += " Singular";
                            break;
                        case 'P':
                            result += " Plural";
                            break;
                        default:
                            throw new KeyNotFoundException(form);
                    }
                    ++i;
                }
                if((new HashSet<char> { 'N', 'V', 'G', 'D', 'A' }).Contains(form[i]))
                {
                    switch (form[i])
                    {
                        case 'N':
                            result += " Nominative";
                            break;
                        case 'V':
                            result += " Vocative";
                            break;
                        case 'G':
                            result += " Genitive";
                            break;
                        case 'D':
                            result += " Dative";
                            break;
                        case 'A':
                            result += " Accusative";
                            break;
                        default:
                            throw new KeyNotFoundException(form);
                    }
                    ++i;
                }
                if ((new HashSet<char> { 'S', 'P' }).Contains(form[i]))
                {
                    switch (form[i])
                    {
                        case 'S':
                            result += " Singular";
                            break;
                        case 'P':
                            result += " Plural";
                            break;
                        default:
                            throw new KeyNotFoundException(form);
                    }
                    ++i;
                }
                if (form.Length > i && (new HashSet<char> { 'M', 'F', 'N' }).Contains(form[i]))
                {
                    switch (form[i])
                    {
                        case 'M':
                            result += " Masculine";
                            break;
                        case 'F':
                            result += " Feminine";
                            break;
                        case 'N':
                            result += " Neuter";
                            break;
                        default:
                            throw new KeyNotFoundException(form);
                    }
                    ++i;
                }
                if (form.Length > i)
                {
                    if (form[i] == '-')
                    {
                        ++i;
                        if (form[i] == 'K')
                            result += " (merged by crasis)";
                    }
                    else
                    {
                        throw new KeyNotFoundException(form);
                    }
                }
                return result;
            }

            if (rmac.Left(3) == "ADV")
            {
                string adverb = "Adverb";
                if (rmac == "ADV")
                    return adverb;

                switch (rmac.Right(1))
                {
                    case "C":
                        return adverb + ": Comparative";
                    case "I":
                        return adverb + ": Interrogative";
                    case "K":
                        return adverb + " (merged by crasis)";
                    case "N":
                        return adverb + ": Negative";
                    default:
                        throw new KeyNotFoundException(rmac);
                }
            }
            else if (rmac.Left(4) == "CONJ")
            {
                string conjunction = "Conjunction";
                if (rmac == "CONJ")
                    return conjunction;
                if (rmac.Right(2) == "-N")
                    return conjunction + ": Negative";
                throw new KeyNotFoundException(rmac);
            }
            else if (rmac.Left(3) == "PRT")
            {
                string particle = "Particle";
                if (rmac == "PRT")
                    return particle;
                if (rmac.Right(2) == "-I")
                    return particle + ": Interrogative";
                if (rmac.Right(2) == "-N")
                    return particle + ": Negative";
                throw new KeyNotFoundException(rmac);
            }
            else if (rmac.Left(4) == "PREP")
            {
                if (rmac == "PREP")
                    return "Preposition";
                throw new KeyNotFoundException(rmac);
            }
            else if (rmac.Left(3) == "INJ")
            {
                if (rmac == "INJ")
                    return "Interjection";
                throw new KeyNotFoundException(rmac);
            }
            else if (rmac.Left(4) == "ARAM")
            {
                if (rmac == "ARAM")
                    return "Aramaic";
                throw new KeyNotFoundException(rmac);
            }
            else if (rmac.Left(3) == "HEB")
            {
                return "Hebrew";
            }
            else if (rmac.Left(5) == "N-PRI")
            {
                if (rmac == "N-PRI")
                    return "Indeclinable Proper Noun";
                throw new KeyNotFoundException(rmac);
            }
            else if (rmac.Left(5) == "A-NUI")
            {
                if (rmac == "A-NUI")
                    return "Indeclinable Numeral (Adjective)";
                throw new KeyNotFoundException(rmac);
            }
            else if (rmac.Left(4) == "N-LI")
            {
                if (rmac == "N-LI")
                    return "Indeclinable Letter (Noun)";
                throw new KeyNotFoundException(rmac);
            }
            else if (rmac.Left(4) == "N-OI")
            {
                if (rmac == "N-OI")
                    return "Indeclinable Noun";
                throw new KeyNotFoundException(rmac);
            }
            else if ((new HashSet<string> { "N", "A", "R", "C", "D", "T", "K", "I", "X", "Q", "F", "S", "P" }).Contains(rmac.Left(1)))
            {
                switch (rmac.Left(1))
                {
                    case "N":
                        return "Noun:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "A":
                        return "Adjective:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "R":
                        return "Relative Pronoun:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "C":
                        return "Reciprocal Pronoun:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "D":
                        return "Demonstrative Pronoun:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "T":
                        return "Definite Article:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "K":
                        return "Correlative Pronoun:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "I":
                        return "Interrogative Pronoun:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "X":
                        return "Indefinite Pronoun:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "Q":
                        return "Correlative/Interrogative Pronoun:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "F":
                        return "Reflexive Pronoun:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "S":
                        return "Possessive Adjective:" + SerializeNounLikeForm(rmac.Substring(2));
                    case "P":
                        return "Personal Pronoun:" + SerializeNounLikeForm(rmac.Substring(2));
                    default:
                        throw new KeyNotFoundException(rmac);
                }
            }
            else if (rmac.Left(1) == "V")
            {
                string result = "Verb:";

                int i = 2;

                switch (rmac[i])
                {
                    case 'P':
                        result += " Present";
                        break;
                    case 'I':
                        result += " Imperfect";
                        break;
                    case 'F':
                        result += " Future";
                        break;
                    case 'A':
                        result += " Aorist";
                        break;
                    case 'R':
                        result += " Perfect";
                        break;
                    case 'L':
                        result += " Pluperfect";
                        break;
                    case '2':
                        ++i;
                        result += " Second";
                        switch (rmac[i])
                        {
                            case 'F':
                                result += " Future";
                                break;
                            case 'A':
                                result += " Aorist";
                                break;
                            case 'R':
                                result += " Perfect";
                                break;
                            case 'L':
                                result += " Pluperfect";
                                break;
                            case 'P':
                                result += " Present";
                                break;
                            default:
                                throw new KeyNotFoundException(rmac);
                        }
                        break;
                    default:
                        throw new KeyNotFoundException(rmac);
                }
                ++i;
                switch (rmac[i])
                {
                    case 'A':
                        result += " Active";
                        break;
                    case 'M':
                        result += " Middle";
                        break;
                    case 'P':
                        result += " Passive";
                        break;
                    case 'E':
                        result += " Middle/Passive";
                        break;
                    case 'D':
                        result += " Middle Deponent";
                        break;
                    case 'O':
                        result += " Passive Deponent";
                        break;
                    case 'N':
                        result += " Middle/Passive Deponent";
                        break;
                    default:
                        throw new KeyNotFoundException(rmac);
                }
                ++i;
                switch (rmac[i])
                {
                    case 'I':
                        result += " Indicative";
                        break;
                    case 'S':
                        result += " Subjunctive";
                        break;
                    case 'O':
                        result += " Optative";
                        break;
                    case 'M':
                        result += " Imperative";
                        break;
                    case 'N':
                        result += " Infinitive";
                        break;
                    case 'P':
                        result += " Participle";
                        break;
                    default:
                        throw new KeyNotFoundException(rmac);
                }
                ++i;
                if (rmac.Length == i)
                    return result;
                ++i;
                switch (rmac[i])
                {
                    case '1':
                        result += " 1st Person";
                        break;
                    case '2':
                        result += " 2nd Person";
                        break;
                    case '3':
                        result += " 3rd Person";
                        break;
                    case 'N':
                        result += " Nominative";
                        break;
                    case 'V':
                        result += " Vocative";
                        break;
                    case 'G':
                        result += " Genitive";
                        break;
                    case 'D':
                        result += " Dative";
                        break;
                    case 'A':
                        result += " Accusative";
                        break;
                    default:
                        throw new KeyNotFoundException(rmac);
                }
                ++i;
                switch (rmac[i])
                {
                    case 'S':
                        result += " Singular";
                        break;
                    case 'P':
                        result += " Plural";
                        break;
                    default:
                        throw new KeyNotFoundException(rmac);
                }
                ++i;
                if (rmac.Length == i)
                    return result;
                switch (rmac[i])
                {
                    case 'M':
                        result += " Masculine";
                        break;
                    case 'F':
                        result += " Feminine";
                        break;
                    case 'N':
                        result += " Neuter";
                        break;
                    default:
                        throw new KeyNotFoundException(rmac);
                }
                ++i;
                if (rmac.Length != i)
                    throw new KeyNotFoundException(rmac);
                return result;
            }
            else
            {
                throw new KeyNotFoundException(rmac);
            }
            return "";
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
