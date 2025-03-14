using MiddlewareExample.Middlewares;

namespace MiddlewareExample.Extensions;

public static class CustomMiddlewareExtension
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CustomMiddleware>();
    }
}