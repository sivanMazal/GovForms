using System.Net;
using System.Text.Json;

namespace GovForms.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // המשך הטיפול בבקשה
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה לא צפויה במערכת הטפסים הלאומית");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // יצירת תשובה אחידה ומקצועית [cite: 2025-12-30]
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "חלה שגיאה פנימית במערכת. פרטי השגיאה תועדו ויועברו לטיפול טכני.",
                Detailed = exception.Message // נוריד את זה בסביבת Production אמיתית
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}