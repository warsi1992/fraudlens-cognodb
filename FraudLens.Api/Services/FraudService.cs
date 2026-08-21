using FraudLens.Api.Contracts;
using FraudLens.Api.DTOs;

namespace FraudLens.Api.Services
{
    public class FraudService : IFraudService
    {
        private readonly IFraudRepository _repository;

        public FraudService(IFraudRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SharedDeviceDto>> GetSharedDevicesAsync(string customerName)
        {
            return await _repository.GetSharedDevicesAsync(customerName);
        }
        public Task<List<SharedIpDto>> GetSharedIpsAsync(string customerName)
        {
            return _repository.GetSharedIpsAsync(customerName);
        }

        public Task<TransferChainDto?> GetTransferChainAsync(string accountNumber)
        {
            return _repository.GetTransferChainAsync(accountNumber);
        }

        public Task<FraudScoreDto> GetFraudScoreAsync(string customerName)
        {
            return _repository.GetFraudScoreAsync(customerName);
        }
    }
}
