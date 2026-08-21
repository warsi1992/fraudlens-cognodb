namespace FraudLens.Api.DTOs
{
    public class FraudScoreDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public List<string> Reasons { get; set; } = [];
    }
}
