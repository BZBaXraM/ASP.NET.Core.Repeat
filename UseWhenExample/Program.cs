var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWhen(context =>
        context.Request.Query.ContainsKey("username"),
    applicationBuilder =>
    {
        applicationBuilder.Use(async (context, next) =>
        {
            if (context.Request.Query["username"] == "root")
            {
                await context.Response.WriteAsync("Hello from Middleware branch\n");
            }
            else
            {
                await context.Response.WriteAsync("User is not found!");
            }

            await next();
        });
    });

app.Run(async context => { await context.Response.WriteAsync("Hello from middleware at main chain"); });

app.Run();