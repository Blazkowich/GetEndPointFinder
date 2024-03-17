namespace EndPointFinder.Data.Context.Settings;

public class MongoSettings : IMongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string ApiCollectionName { get; set; }
    public string EndpointCollectionName { get; set; }
}

