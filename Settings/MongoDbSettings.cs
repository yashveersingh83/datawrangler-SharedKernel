namespace SharedKernel.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; init; } = "localhost";
        public int Port { get; init; } = 27017;
        public string DatabaseName { get; init; } = "default";
        public string Username { get; init; }
        public string Password { get; init; }

        public string ConnectionString =>
            string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password)
                ? $"mongodb://{Host}:{Port}"
                : $"mongodb://{Username}:{Password}@{Host}:{Port}/?authSource=admin";
    }
}