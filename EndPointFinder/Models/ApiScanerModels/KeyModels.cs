using Newtonsoft.Json;

namespace EndPointFinder.Models.ApiScanerModels;

public class KeyModels
{
    [JsonProperty("RequestUrl")]
    public string RequestUrl { get; set; }

    [JsonProperty("InitiatorUrl")]
    public string InitiatorUrl { get; set; }
}
