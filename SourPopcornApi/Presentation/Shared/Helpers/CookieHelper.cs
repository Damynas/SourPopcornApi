using Microsoft.AspNetCore.Http;

namespace Presentation.Shared.Helpers;

public static class CookieHelper
{
    public static string? GetCookie(HttpContext httpContext, string name)
    {
        return httpContext.Request.Cookies.TryGetValue(name, out var value) ? value : null;
    }

    public static void SetCookie(HttpContext httpContext, string name, string value)
    {
        httpContext.Response.Cookies.Append(name, value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true
        });
    }

    public static void RemoveCookie(HttpContext httpContext, string name)
    {
        httpContext.Response.Cookies.Delete(name);
    }
}
