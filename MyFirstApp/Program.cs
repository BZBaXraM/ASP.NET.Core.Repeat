using Microsoft.AspNetCore.WebUtilities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

app.MapPost("/get-name", (string name) => Results.Json(name));

app.MapGet("/add", (int n1, int n2) => Results.Json(n1 + n2));

app.Run(async context =>
{
    string[] operations = ["add", "sub", "mult", "div"];

    context.Response.Headers.ContentType = "text/html";
    if (context.Request.Method == "GET")
    {
        if (context.Request.Query.ContainsKey("num1") && context.Request.Query.ContainsKey("num2"))
        {
            var num1 = context.Request.Query["num1"];
            var num2 = context.Request.Query["num2"];
            var op = context.Request.Query["op"];
            int n1 = 0;
            int n2 = 0;

            if (op == "add")
            {
                await context.Response.WriteAsync($"<h1>{n1 + n2}</h1>");
                // await context.Response.WriteAsync(n1 + n2);
            }
        }
    }
});


app.Run(async context =>
{
    // context.Response.Headers.ContentType = "text/html";
    // if (context.Request.Method == "GET")
    // {
    //     if (context.Request.Query.ContainsKey("id"))
    //     {
    //         string? id = context.Request.Query["id"];
    //         await context.Response.WriteAsync($"<p>{id}</p>");
    //     }
    //
    //     if (context.Request.Query.ContainsKey("name"))
    //     {
    //         string? name = context.Request.Query["name"];
    //         await context.Response.WriteAsync($"<p>{name}</p>");
    //     }
    // }

    // if (context.Request.Headers.TryGetValue("User-Agent", out var userAgent))
    // {
    //     await context.Response.WriteAsync($"<p>{userAgent}</p>");
    // }

    using StreamReader streamReader = new(context.Request.Body);
    var body = await streamReader.ReadToEndAsync();

    var query = QueryHelpers.ParseQuery(body);

    // if (context.Request.Method is "GET")
    if (query.ContainsKey("firstName") && query.ContainsKey("lastName"))
    {
        var firstName = query["firstName"][0];
        var lastName = query["lastName"][0];
        await context.Response.WriteAsync($"<p>{firstName}</p>");
        await context.Response.WriteAsync($"<p>{lastName}</p>");
    }
});