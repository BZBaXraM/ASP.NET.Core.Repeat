namespace MiddlewareExample.Middlewares;

public class HelloCustomMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Query.ContainsKey("firstname")
            && context.Request.Query.ContainsKey("lastname"))
        {
            var query = context.Request.Query["firstname"] + " " + context.Request.Query["lastname"];
            await context.Response.WriteAsync($"{query}\n");
        }

        await next(context);
    }
}