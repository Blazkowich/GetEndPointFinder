using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace EndPointFinder.Models.ApiScanerModels;

public class KeyModels
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("RequestUrl")]
    public string RequestUrl { get; set; }

    [JsonProperty("InitiatorUrl")]
    public string InitiatorUrl { get; set; }

    public KeyModels()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }
}
