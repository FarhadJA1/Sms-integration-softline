using SoftlineIntegrationSystem.Models;

namespace SoftlineIntegrationSystem.Services;

public interface IIikoService
{
    Task<string?> GetTokenAsync(string apikey);
    Task<CustomerInfoResponse?> GetCustomerInfoAsync(string customerId, string organizationId, string token);
}
