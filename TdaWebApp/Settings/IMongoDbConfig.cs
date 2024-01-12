namespace TdaWebApp.Settings
{
    public interface IMongoDbConfig
    {
        string Name { get; }
        string ConnectionString { get; }
    }
}
