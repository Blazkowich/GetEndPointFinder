using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace EndPointFinder.Models.EndpointScanerModels;

public class EndpointModels
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("Type")]
    public string Type { get; set; }

    [JsonProperty("Endpoint")]
    public string Endpoint { get; set;}

    [JsonProperty("Message")]
    public string Message { get; set;}

    [JsonProperty("Amount")]
    public int Amount { get; set;}

    public EndpointModels()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }
}
