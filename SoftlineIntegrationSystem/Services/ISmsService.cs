using SoftlineIntegrationSystem.Models;

namespace SoftlineIntegrationSystem.Services;

public interface ISmsService
{
    Task<SoftlineResponse> SendSms(SoftlineRequest request);
}
