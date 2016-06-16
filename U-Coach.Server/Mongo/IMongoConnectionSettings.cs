namespace PVDevelop.UCoach.Server.Mongo
{
    public interface IMongoConnectionSettings
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
        string CollectionName { get; }
    }
}
