using Microsoft.AspNetCore.Http;

public class ThemeService
{
    public bool IsDarkMode(HttpContext httpContext)
    {
        if (httpContext?.Items.TryGetValue("IsDarkMode", out var isDarkMode) == true)
        {
            return (bool)isDarkMode;
        }

        return false; // default to light mode
    }
}