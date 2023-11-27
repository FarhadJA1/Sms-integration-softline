using System.Security.Claims;

namespace SoftlineIntegrationSystem.Helpers;

public static class HttpContextExtensions
{
    public static string? GetUserEmail(this HttpContext context)
    {
        ClaimsIdentity? identity = context.User.Identity as ClaimsIdentity;

        return identity?.FindFirst("Email")?.Value;
    }

    public static void AddTotalCountHeader(this HttpResponse response, int total)
    {
        response.Headers.Add("Access-Control-Expose-Headers", "totalCount");
        response.Headers.Add("totalCount", total.ToString());
    }
}
