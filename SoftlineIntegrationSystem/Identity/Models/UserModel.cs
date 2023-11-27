using System.ComponentModel.DataAnnotations;

namespace SoftlineIntegrationSystem.Identity.Models;

public class UserModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    [EmailAddress]
    public string Email { get; set; } = default!;
    public bool IsAdmin { get; set; }
}