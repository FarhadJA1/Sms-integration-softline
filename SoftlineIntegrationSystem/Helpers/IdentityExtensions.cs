using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SoftlineIntegrationSystem.Data;
using SoftlineIntegrationSystem.Identity.Entities;
using SoftlineIntegrationSystem.Identity.Services;
using System.Text;

namespace SoftlineIntegrationSystem.Helpers;

public static class IdentityExtensions
{

    public static void AddCustomIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        string appSecret = configuration["Secret"];
        byte[] key = Encoding.ASCII.GetBytes(appSecret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        IUserService userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        int userId = int.Parse(context!.Principal?.Identity?.Name!);
                        User? user = userService.GetById(userId);
                        if (user == null)
                            context.Fail("Unauthorized");

                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        services.AddScoped<IUserService, UserService>();
    }


    public static void ApplyMigrations(this IServiceCollection services)
    {
        ServiceProvider? provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (context.Database.CanConnect() && context.Users.Any())
        {
            return;
        }
        try
        {
            context.Database.EnsureCreated();
        }
        catch (Exception)
        { }
    }
}