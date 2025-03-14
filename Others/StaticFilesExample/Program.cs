var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("/", async context => { await context.Response.WriteAsync("Hello"); });
});

app.UseStaticFiles();
app.Run();