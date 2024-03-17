using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace EndPointFinder.Models.EndpointScanerModels;

public class EndpointScanerRootModels
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("Endpoint")]
    public HashSet<EndpointModels> Endpoints { get; set; }

    [JsonProperty("Messages")]
    public List<string> Messages { get; set; }
}
