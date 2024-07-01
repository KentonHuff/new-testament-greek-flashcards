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
    }
}
