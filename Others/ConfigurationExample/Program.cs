using ConfigurationExample;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection("Api"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "My API"); });

    app.MapScalarApiReference();
}

app.MapControllers();
app.UseRouting();


// app.UseEndpoints(endpoints =>
// {
//     endpoints.Map("/", async context =>
//     {
//         await context.Response.WriteAsync(app.Configuration["MyKey"] + '\n');
//
//         await context.Response.WriteAsync(app.Configuration.GetValue<string>("MyKey"));
//     });
// });
app.Run();