namespace FraudLens.Api.Models
{
    public class Device
    {
        public Guid DeviceId { get; set; }

        public string DeviceName { get; set; } = string.Empty;

        public string DeviceType { get; set; } = string.Empty;
    }
}
