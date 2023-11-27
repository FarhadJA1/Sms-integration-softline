using SoftlineIntegrationSystem.Identity.Entities;

namespace SoftlineIntegrationSystem.Identity.Services;

public interface IUserService
{
    User? Authenticate(string email, string password);
    IEnumerable<User> GetAll();
    User? GetById(int id);
    User? GetByEmail(string email);
    User Create(User user, string password);
    void Update(User user, string? password = null);
    void Delete(int id);
}
