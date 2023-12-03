using Microsoft.AspNetCore.Http;

namespace Presentation.Shared.Helpers;

public static class CookieHelper
{
    public static string? GetCookie(HttpContext httpContext, string name)
    {
        return httpContext.Request.Cookies.TryGetValue(name, out var value) ? value : null;
    }

    public static void SetCookie(HttpContext httpContext, string name, string value, int expirationTimeInMinutes)
    {
        httpContext.Response.Cookies.Append(name, value, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(expirationTimeInMinutes),
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None
        });
    }

    public static void RemoveCookie(HttpContext httpContext, string name)
    {
        httpContext.Response.Cookies.Delete(name, new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddMinutes(-1),
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None
        });
    }
}
