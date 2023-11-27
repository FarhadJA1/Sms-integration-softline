using Microsoft.EntityFrameworkCore;
using SoftlineIntegrationSystem.Identity.Entities;
using SoftlineIntegrationSystem.Models.Entities;

namespace SoftlineIntegrationSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreatePasswordHash("Ramin123", out byte[]? passwordHash, out byte[]? passwordSalt);

        User user = new()
        {
            Id = 1,
            Email = "ramin@example.com",
            FirstName = "Ramin",
            LastName = "Guliyev",
            IsAdmin = true,
            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash
        };
        modelBuilder.Entity<User>()
            .HasData(user);
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            string connectionString = configuration.GetConnectionString("Default");
            optionsBuilder.UseSqlServer(connectionString);
        }
        base.OnConfiguring(optionsBuilder);
    }
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Restaurant> Restaurants { get; set; } = default!;
    public DbSet<Venue> Venues { get; set; } = default!;
    public DbSet<Models.Entities.Action> Actions { get; set; } = default!;
    public DbSet<Log> Logs { get; set; } = default!;
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

        using System.Security.Cryptography.HMACSHA512 hmac = new();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
}
