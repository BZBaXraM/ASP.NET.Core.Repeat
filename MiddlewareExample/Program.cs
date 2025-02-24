using MiddlewareExample.Extensions;
using MiddlewareExample.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<CustomMiddleware>();
var app = builder.Build();

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Hello\n");
    await next(context);
});

app.UseCustomMiddleware();
app.UseMiddleware<HelloCustomMiddleware>();

app.Run(async context =>
{
    await context.Response.WriteAsync("Hello again\n");
});

app.Run();