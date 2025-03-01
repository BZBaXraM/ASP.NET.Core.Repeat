var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// app.Use(async (context, next) =>
// {
//     var endpoint = context.GetEndpoint();
//     await context.Response.WriteAsync($"Endpoint: {endpoint?.DisplayName}\n");
//     await next(context);
// });


app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // files/text.txt
    endpoints.Map("files/{filename}.{extension}",
        async context =>
        {
            var fileName = Convert.ToString(context.Request.RouteValues["filename"]);
            var extension = Convert.ToString(context.Request.RouteValues["extension"]);
            await context.Response.WriteAsync($"In files - {fileName} - {extension}");
        });

    // employee/profile/gulya
    endpoints.Map("employee/profile/{EmployeeName:length(4,7):alpha?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("EmployeeName"))
        {
            var name = Convert.ToString(context.Request.RouteValues["employeename"]);
            await context.Response.WriteAsync($"In Employee profile - {name}");
        }
        else
        {
            await context.Response.WriteAsync("Employee profile - name is not supplied");
        }
    });

    // products/details/97
    endpoints.Map("products/details/{id:int?}", async context =>
    {
        if (context.Request.RouteValues.TryGetValue("id", out var value))
        {
            var id = Convert.ToInt32(value);
            await context.Response.WriteAsync($"Product details - {id}");
        }
        else
        {
            await context.Response.WriteAsync("Products details - id is not supplied");
        }
    });

    // daily-digest-report/2030-06-30
    endpoints.Map("daily-digest-report/{reportDate:datetime}", async context =>
    {
        var reportDate = Convert.ToDateTime(context.Request.RouteValues["reportDate"]);
        await context.Response.WriteAsync($"In daily-digest-report - {reportDate.ToShortDateString()}");
    });

    // cities/17b66cfe-e848-415b-b62e-f29ffc831af4
    endpoints.Map("cities/{cityId:guid}", async context =>
    {
        var id = Guid.Parse(context.Request.RouteValues["cityId"].ToString()!);
        await context.Response.WriteAsync($"City Information - {id}");
    });

    // sales-report/2030/apr
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:regex" +
                  "(^(apr|jul|oct|jan)$)}", async context =>
    {
        var year = Convert.ToInt32(context.Request.RouteValues["year"]);
        var month = context.Request.RouteValues["month"].ToString()!;

        await context.Response.WriteAsync($"Sales report - {year} - {month}");
    });
});

app.Run(async context => { await context.Response.WriteAsync($"Request received: {context.Request.Path}"); });

app.Run();