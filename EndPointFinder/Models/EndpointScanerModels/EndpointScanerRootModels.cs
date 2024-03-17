using Newtonsoft.Json;

namespace EndPointFinder.Models.EndpointScanerModels;

public class EndpointScanerRootModels
{
    [JsonProperty("Endpoint")]
    public HashSet<EndpointModels> Endpoints { get; set; }

    [JsonProperty("Messages")]
    public List<string> Messages { get; set; }
}
