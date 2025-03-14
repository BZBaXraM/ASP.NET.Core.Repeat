namespace MiddlewareExample.Middlewares;

public class CustomMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await context.Response.WriteAsync("Middleware is started...\n");
        await next(context);
        await context.Response.WriteAsync("Middleware is ended...");
    }
}