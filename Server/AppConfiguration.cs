
public class GeneralRule
{
    public string Endpoint { get; set; }
    public string Period { get; set; }
    public int Limit { get; set; }
}

public class IpRateLimiting
{
    public bool EnableEndpointRateLimiting { get; set; }
    public bool StackBlockedRequests { get; set; }
    public string RealIpHeader { get; set; }
    public string ClientIdHeader { get; set; }
    public int HttpStatusCode { get; set; }
    public List<GeneralRule> GeneralRules { get; set; }
}

public class AppConfiguration
{
    public IpRateLimiting IpRateLimiting { get; set; }
}

