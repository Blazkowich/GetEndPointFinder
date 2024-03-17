using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace EndPointFinder.Models.ApiScanerModels;

public class ApiScanerRootModels
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [JsonProperty("Api")]
    public HashSet<ApiModels> Apis { get; set; }

    [JsonProperty("Key")]
    public HashSet<KeyModels> Keys { get; set; }
}
