namespace SoftlineIntegrationSystem.Models;

public class WebhookSmsBody
{
    public string? sender { get; set; }
    public string? text { get; set; }
    public string? phone { get; set; }
    public string? id { get; set; }
    public string? organizationId { get; set; }
    public string? uocId { get; set; }
    public int? notificationType { get; set; }
    public string? customerId { get; set; }
    public string? transactionType { get; set; }
    public string? subscriptionPassword { get; set; }
    public string? changedOn { get; set; }
    public string? programId { get; set; }
    public bool? isExcluded { get; set; }
    public string? magnetCardId { get; set; }
    public string? number { get; set; }
    public string? track { get; set; }
    public bool? isDeleted { get; set; }
}
