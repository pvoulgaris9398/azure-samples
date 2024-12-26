using dotenv.net;

namespace Domain
{
    public class Config
    {
        static Config()
        {
            DotEnv.Load();
        }
        public static string ConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("AZURE_QUEUE_STORAGE_CONNECTION_STRING") ?? throw new Exception("Environment variable: 'AZURE_QUEUE_STORAGE_CONNECTION_STRING' not found!");
            }
        }
    }
}