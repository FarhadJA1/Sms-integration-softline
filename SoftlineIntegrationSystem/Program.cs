using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SoftlineIntegrationSystem.Data;
using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Repositories;
using SoftlineIntegrationSystem.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpClient();
    builder.Services.AddMemoryCache();
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "SMS Integration Service Created by Ramin Guliyev", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
        });
    });
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    });
    builder.Services.AddCustomIdentity(builder.Configuration);
    builder.Services.ApplyMigrations();
    builder.Services.AddScoped<IActionRepository, ActionRepository>();
    builder.Services.AddScoped<ILogRepository, LogRepository>();
    builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
    builder.Services.AddScoped<IVenueRepository, VenueRepository>();
    builder.Services.AddScoped<ISmsService, SmsService>();
    builder.Services.AddTransient<IIikoService, IikoService>();
    builder.Services.AddCors();
}

WebApplication app = builder.Build();

{
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}


