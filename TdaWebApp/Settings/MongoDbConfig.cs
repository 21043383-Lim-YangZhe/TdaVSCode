namespace TdaWebApp.Settings
{
    public class MongoDbConfig : IMongoDbConfig
    {
        public string Name { get; init; }
        public string Host { get; init; }
        public int Port { get; init; }
        //public string ConnectionString => $"mongodb://{Host}:{Port}";
        public string ConnectionString { get; init; }
    }
}

