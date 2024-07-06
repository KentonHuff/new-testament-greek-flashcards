namespace FlashcardGen.Common
{
    public static class Constants
    {
        public const int NumberOfWordOccurrences = 138013;
        public static class LocalFiles
        {
            public const string ConfigFileName = "config.json";

            public const string OpenGreekNewTestamentFileName = "OpenGNT_version3_3.csv";
            public const string InputFilesPath = "./LocalFiles/Input/";
            public const string SQLiteDb = "./LocalFiles/Input/OpenGNTSQLite.db";
            public const string OutputFilePath = "./LocalFiles/Output/AnkiFlashcards.txt";
        }

        public static class ConfigPaths
        {
            public const string OpenGNTBaseTextZipURL = "Input:OpenGNTBaseTextZipURL";
            public const string ReadDbFromDisk = "Input:ReadDbFromDisk";
            public const string ReadUncompressedOpenGNTBaseText = "Input:ReadUncompressedOpenGNTBaseText";
            public const string WriteDbToDisk = "Output:WriteDbToDisk";
        }

        public static class ConnectionStrings
        {
            public const string InMemory = "DataSource=:memory:";
            public const string OnDisk = $"DataSource={LocalFiles.SQLiteDb}";
        }

        public static readonly Dictionary<char,string> GreekPreOccurrencePunctuationMapping = new Dictionary<char, string>()
        {
            { '¬', "<br>&nbsp;&nbsp;&nbsp;&nbsp;" },
            { '[', "[" },
        };

        public static readonly Dictionary<char, string> GreekPostOccurrencePunctuationMapping = new Dictionary<char, string>()
        {
            { ',', ", " },
            { '.', ". " },
            { '¶', "<br>" },
            { '·', "· " },
            { ';', "; " },
            { '—', " — " },
            { ']', "]" },
        };

        public static readonly Dictionary<char, string> EnglishPreOccurrencePunctuationMapping = new Dictionary<char, string>()
        {
            { '¬', "<br>&nbsp;&nbsp;&nbsp;&nbsp;" },
            { '[', "[" },
        };

        public static readonly Dictionary<char, string> EnglishPostOccurrencePunctuationMapping = new Dictionary<char, string>()
        {
            { ',', " " },
            { '.', " " },
            { '¶', "<br>" },
            { '·', " " },
            { ';', " " },
            { '—', " " },
            { ']', "]" },
        };

        public static readonly Dictionary<int, string> BookNumberToName = new Dictionary<int, string>
        {
            { 40, "Matthew" },
            { 41, "Mark" },
            { 42, "Luke" },
            { 43, "John" },
            { 44, "Acts" },
            { 45, "Romans" },
            { 46, "1 Corinthians" },
            { 47, "2 Corinthians" },
            { 48, "Galatians" },
            { 49, "Ephesians" },
            { 50, "Philippians" },
            { 51, "Colossians" },
            { 52, "1 Thessalonians" },
            { 53, "2 Thessalonians" },
            { 54, "1 Timothy" },
            { 55, "2 Timothy" },
            { 56, "Titus" },
            { 57, "Philemon" },
            { 58, "Hebrews" },
            { 59, "James" },
            { 60, "1 Peter" },
            { 61, "2 Peter" },
            { 62, "1 John" },
            { 63, "2 John" },
            { 64, "3 John" },
            { 65, "Jude" },
            { 66, "Revelation" },
        };
    }
}
