using CountriesApp.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Country> countries =
[
    new(1, "Azerbaijan"),
    new(2, "Turkey"),
    new(3, "Russian"),
    new(4, "America")
];

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("/countries", async context =>
    {
        foreach (var item in countries)
        {
            await context.Response.WriteAsync($"{item.Name}\n");
        }
    });
    endpoints.Map("/countries/{id:int?}", async context =>
    {
        foreach (var item in countries)
        {
            if (context.Request.RouteValues.TryGetValue("id", out var value))
            {
                var id = Convert.ToInt32(value);

                if (id == item.Id)
                {
                    await context.Response.WriteAsync($"{item.Name}\n");
                }
            }
        }
    });
});

app.Run();