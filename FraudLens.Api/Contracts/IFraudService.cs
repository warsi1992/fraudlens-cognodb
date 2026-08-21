using FraudLens.Api.DTOs;

namespace FraudLens.Api.Contracts
{
    public interface IFraudService
    {
        Task<List<SharedDeviceDto>> GetSharedDevicesAsync(string customerName);
        Task<List<SharedIpDto>> GetSharedIpsAsync(string customerName);

        Task<TransferChainDto?> GetTransferChainAsync(string accountNumber);

        Task<FraudScoreDto> GetFraudScoreAsync(string customerName);
    }
}
