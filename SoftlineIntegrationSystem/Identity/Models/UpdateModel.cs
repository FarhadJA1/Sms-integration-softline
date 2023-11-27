using System.ComponentModel.DataAnnotations;

namespace SoftlineIntegrationSystem.Identity.Models;

public class UpdateModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsAdmin { get; set; }
}
