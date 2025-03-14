using LoginMiddlewareTask.Middlewares;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddScoped<LoginMiddleware>();
var app = builder.Build();


// app.Run(async context => { await context.Response.WriteAsync("No Response!"); });

app.UseMiddleware<LoginMiddleware>();

app.Run();