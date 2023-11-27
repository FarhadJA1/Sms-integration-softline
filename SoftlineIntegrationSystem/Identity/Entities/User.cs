using System.ComponentModel.DataAnnotations;

namespace SoftlineIntegrationSystem.Identity.Entities;

public class User
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = default!;
    [Required]
    public string LastName { get; set; } = default!;
    [Required]
    public string Email { get; set; } = default!;
    public bool IsAdmin { get; set; }
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
}
