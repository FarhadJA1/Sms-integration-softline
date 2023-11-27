using SoftlineIntegrationSystem.Data;
using SoftlineIntegrationSystem.Identity.Entities;
using SoftlineIntegrationSystem.Identity.Helpers;
using System.Security.Cryptography;

namespace SoftlineIntegrationSystem.Identity.Services;
public class UserService : IUserService
{
    private AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public User? Authenticate(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return null;

        User? user = _context.Users.SingleOrDefault(x => x.Email == email);

        // check if username exists
        if (user == null)
            return null;

        // check if password is correct
        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            return null;

        // authentication successful
        return user;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User? GetById(int id)
    {
        return _context.Users.Find(id);
    }

    public User Create(User user, string password)
    {
        // validation
        if (string.IsNullOrWhiteSpace(password))
            throw new AppException("Password is required");

        if (_context.Users.Any(x => x.Email == user.Email))
            throw new AppException("Username \"" + user.Email + "\" is already taken");

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public void Update(User userParam, string? password = null)
    {
        User? user = _context.Users.Find(userParam.Id);

        if (user == null)
            throw new AppException("User not found");

        // update username if it has changed
        if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
        {
            // throw error if the new username is already taken
            if (_context.Users.Any(x => x.Email == userParam.Email))
                throw new AppException("Username " + userParam.Email + " is already taken");

            user.Email = userParam.Email;
        }

        // update user properties if provided
        if (!string.IsNullOrWhiteSpace(userParam.FirstName))
            user.FirstName = userParam.FirstName;

        user.IsAdmin = userParam.IsAdmin;

        if (!string.IsNullOrWhiteSpace(userParam.LastName))
            user.LastName = userParam.LastName;

        // update password if provided
        if (!string.IsNullOrWhiteSpace(password))
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }

        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        User? user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }

    // private helper methods

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (password == null) throw new ArgumentNullException(nameof(password));
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

        using HMACSHA512 hmac = new();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        if (password == null) throw new ArgumentNullException(nameof(password));
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
        if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
        if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

        using (HMACSHA512 hmac = new(storedSalt))
        {
            byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
        }

        return true;
    }

    public User? GetByEmail(string email)
    {
        return _context.Users.Where(x => x.Email == email).FirstOrDefault();
    }
}