using System.ComponentModel.DataAnnotations;

namespace SoftlineIntegrationSystem.Identity.Models;

public class RegisterModel
{
    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public bool IsAdmin { get; set; }

    [Required]
    public string? Password { get; set; }
}
