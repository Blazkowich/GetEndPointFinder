namespace EndPointFinder.Models.ApiScanerModels;

public class ApiScanerModels
{
    public string Id { get; set; }

    public HashSet<ApiResponseModels> Apis { get; set; }

    public HashSet<KeyResponseModels> Keys { get; set; }

    public class ApiResponseModels
    {
        public string Id { get; set; }

        public string RequestUrl { get; set; }

        public string InitiatorUrl { get; set; }
    }

    public class KeyResponseModels
    {
        public string Id { get; set; }

        public string RequestUrl { get; set; }

        public string InitiatorUrl { get; set; }
    }
}
