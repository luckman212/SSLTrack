using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class ThemeMiddleware
{
    private readonly RequestDelegate _next;

    public ThemeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue("IsDarkMode", out var value))
        {
            context.Items["IsDarkMode"] = bool.TryParse(value, out var isDarkMode) && isDarkMode;
        }
        else
        {
            context.Items["IsDarkMode"] = false;
        }

        await _next(context);
    }
}