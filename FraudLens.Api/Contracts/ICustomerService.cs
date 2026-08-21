using FraudLens.Api.DTOs;

namespace FraudLens.Api.Contracts
{
    public interface ICustomerService
    {
        Task<List<CustomerSearchDto>> SearchAsync(string name);
        Task<CustomerDetailsDto?> GetDetailsAsync(string customerId);
    }
}
