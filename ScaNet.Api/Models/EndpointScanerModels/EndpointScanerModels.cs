namespace EndPointFinder.Models.EndpointScanerModels;

public class EndpointScanerModels
{
    public string Id { get; set; }

    public HashSet<EndpointResponseModels> Endpoints { get; set; }

    public List<string> Messages { get; set; }

    public class EndpointResponseModels
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Endpoint { get; set; }

        public string Message { get; set; }

        public int Amount { get; set; }
    }
}
