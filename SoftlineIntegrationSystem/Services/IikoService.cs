using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Models;
using System.Net.Http.Headers;

namespace SoftlineIntegrationSystem.Services;

public class IikoService : IIikoService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IikoService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<CustomerInfoResponse?> GetCustomerInfoAsync(string customerId, string organizationId, string token)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using HttpResponseMessage response = await httpClient
            .PostAsJsonAsync($"{Constants.IikoTransportUrl}/api/1/loyalty/iiko/customer/info", new CustomerInfoRequest
            {
                id = customerId,
                organizationId = organizationId
            });
        response.EnsureSuccessStatusCode();
        CustomerInfoResponse? responseBody = await response.Content.ReadFromJsonAsync<CustomerInfoResponse>();
        return responseBody;
    }

    public async Task<string?> GetTokenAsync(string apikey)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();
        using HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{Constants.IikoTransportUrl}/api/1/access_token", new TokenRequest { apiLogin = apikey });
        response.EnsureSuccessStatusCode();
        TokenResponse? responseBody = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return responseBody?.token;
    }

}
