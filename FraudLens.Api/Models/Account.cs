namespace FraudLens.Api.Models
{
    public class Account
    {
        public Guid AccountId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public decimal Balance { get; set; }
    }
}
