namespace FlashcardGen.Common
{
    public static class Constants
    {
        public const int NumberOfWordOccurrences = 138013;
        public static class LocalFiles
        {
            public const string ConfigFileName = "config.json";

            public const string OpenGreekNewTestamentFileName = "OpenGNT_version3_3.csv";
            public const string OpenGreekNewTestamentPath = "./LocalFiles/Input/";
        }

        public static class ConfigPaths
        {
            public const string OpenGNTBaseTextZipURL = "Input:OpenGNTBaseTextZipURL";
        }

        public static class ConnectionStrings
        {
            public const string InMemory = "DataSource=:memory:";
            public const string OnDisk = "DataSource=./LocalFiles/Input/OpenGNTSQLite.db";
        }
    }
}
