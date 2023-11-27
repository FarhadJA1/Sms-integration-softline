using SoftlineIntegrationSystem.Models;
using SoftlineIntegrationSystem.Repositories;
using System.Net;
using System.Text.Json;

namespace SoftlineIntegrationSystem.Helpers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                ILogRepository? logRepository = context.RequestServices.GetService<ILogRepository>();
                if (logRepository is not null)
                {
                    await logRepository.AddAsync(new Models.Entities.Log
                    {
                        CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                        Description = ex.Message,
                        More = ex.StackTrace
                    });
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ErrorResponse response = new ErrorResponse(context.Response.StatusCode, "Internal Server Error", "Admins informed, they solve the problem as soon as possible");

                JsonSerializerOptions option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                string json = JsonSerializer.Serialize(response, option);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
