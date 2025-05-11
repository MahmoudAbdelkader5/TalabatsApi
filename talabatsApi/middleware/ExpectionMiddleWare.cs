using System.Text.Json;
using talabatsApi.Error;

namespace talabatsApi.middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        public RequestDelegate Next { get; }
        public IHostEnvironment Env { get; }

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            Next = next;
            _logger = logger;
            Env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var response = Env.IsDevelopment()? new apiExpectionResponse(500, ex.Message, ex.StackTrace): new apiExpectionResponse(500);

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}