using System.ComponentModel.DataAnnotations;

namespace SoftlineIntegrationSystem.Identity.Models;

public class AuthenticateModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}
