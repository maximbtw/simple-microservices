using System.Globalization;

namespace PizzeriaApi.WebApi.Middleware;

internal class RequestLocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var langHeader = context.Request.Headers["Accept-Language"].ToString();

        if (!string.IsNullOrEmpty(langHeader))
        {
            try
            {
                var culture = new CultureInfo(langHeader);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            catch (CultureNotFoundException)
            {
                // Игнорируем, если культура не найдена
            }
        }

        await _next(context);
    }
}