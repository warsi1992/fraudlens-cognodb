namespace FraudLens.Api.Models
{
    public class IPAddress
    {
        public Guid IPAddressId { get; set; }

        public string Address { get; set; } = string.Empty;
    }
}
