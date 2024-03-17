namespace EndPointFinder.Data.Context.Settings;

public interface IMongoSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    string ApiCollectionName { get; set; }
    string EndpointCollectionName { get; set; }
}

