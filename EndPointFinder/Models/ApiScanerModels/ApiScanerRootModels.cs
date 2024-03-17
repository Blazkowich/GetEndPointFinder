using Newtonsoft.Json;

namespace EndPointFinder.Models.ApiScanerModels;

public class ApiScanerRootModels
{
    [JsonProperty("Api")]
    public HashSet<ApiModels> Api { get; set; }

    [JsonProperty("Key")]
    public HashSet<KeyModels> Key { get; set; }
}
