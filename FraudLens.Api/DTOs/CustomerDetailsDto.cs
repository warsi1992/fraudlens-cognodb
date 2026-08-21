namespace FraudLens.Api.DTOs
{
    public class CustomerDetailsDto
    {
        public string? CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<string>? Accounts { get; set; }
        public List<string>? Devices { get; set; }
        public List<string>? IpAddresses { get; set; }
        public string? City { get; set; }
    }
}
