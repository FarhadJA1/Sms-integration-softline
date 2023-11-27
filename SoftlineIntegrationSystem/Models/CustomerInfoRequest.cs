namespace SoftlineIntegrationSystem.Models;

public class CustomerInfoRequest
{
    public string? id { get; set; }
    public string type { get; set; } = "id";
    public string? organizationId { get; set; }
}
public class CustomerInfoResponse
{
    public string? id { get; set; }
    public string? referrerId { get; set; }
    public string? name { get; set; }
    public string? surname { get; set; }
    public string? middleName { get; set; }
    public string? comment { get; set; }
    public string? phone { get; set; }
    public string? cultureName { get; set; }
    public string? birthday { get; set; }
    public string? email { get; set; }
    public bool isDeleted { get; set; }
}
