using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Models;
using System.Text;

namespace SoftlineIntegrationSystem.Services;

public class SmsService : ISmsService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SmsService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<SoftlineResponse> SendSms(SoftlineRequest request)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using HttpClient httpClient = _httpClientFactory.CreateClient();
        using HttpResponseMessage response = await httpClient.GetAsync($"{Constants.SoftlineUrl}/sendsms?user={request.Username}&password={request.Apikey}&gsm={request.Number}&from={request.SenderName}&text={request.Text}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        return SoftlineResponseBuilder(responseBody);
    }
    //errno=100&errtext=OK&message_id=815845585&charge=1&balance=57376
    private SoftlineResponse SoftlineResponseBuilder(string response)
    {
        SoftlineResponse softlineResponse = new();

        string[] arr = response.Split('&');

        foreach (string item in arr)
        {
            string[] arr2 = item.Split('=');
            switch (arr2[0])
            {
                case "errno":
                    softlineResponse.ErrNo = int.Parse(arr2[1]);
                    break;
                case "errtext":
                    softlineResponse.ErrText = arr2[1];
                    break;
                case "message_id":
                    softlineResponse.MessageId = long.Parse(arr2[1]);
                    break;
                case "charge":
                    softlineResponse.Charge = int.Parse(arr2[1]);
                    break;
                case "balance":
                    softlineResponse.Balance = int.Parse(arr2[1]);
                    break;
                default:
                    break;
            }
        }

        return softlineResponse;
    }
}