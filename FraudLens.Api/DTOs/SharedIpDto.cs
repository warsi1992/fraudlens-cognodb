namespace FraudLens.Api.DTOs
{
    public class SharedIpDto
    {
        public string CustomerName { get; set; } = string.Empty;

        public string IpAddress { get; set; } = string.Empty;

        public string SharedWith { get; set; } = string.Empty;
    }
}
