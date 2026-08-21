namespace FraudLens.Api.DTOs
{
    public class TransferChainDto
    {
        public string StartAccount { get; set; } = string.Empty;
        public List<string> Path { get; set; } = [];
    }
}
