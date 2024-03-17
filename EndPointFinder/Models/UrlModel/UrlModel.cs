using Newtonsoft.Json;

namespace EndPointFinder.Models.UrlModel;

public class UrlModel
{
    [JsonProperty("Url")]
    public string Url { get; set; }

    [JsonProperty("Message")]
    public string Message { get; set; }
}
