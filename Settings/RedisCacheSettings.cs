namespace SharedKernel.Settings
{
    public class RedisCacheSettings
    {
        public string Host { get; init; }

        public int Port { get; init; }
        public string Password { get; init; }

        public string ConnectionString => $"{Host}:{Port}";
    }
}