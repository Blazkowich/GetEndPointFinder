using Newtonsoft.Json;

namespace EndPointFinder.Models.ApiScanerModels;

public class ApiScanerRootModels
{
    [JsonProperty("Api")]
    public HashSet<ApiModels> Apis { get; set; }

    [JsonProperty("Key")]
    public HashSet<KeyModels> Keys { get; set; }
}
