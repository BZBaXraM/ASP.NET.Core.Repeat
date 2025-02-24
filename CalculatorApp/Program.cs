var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

app.Run(async context =>
{
    if (context.Request.Method == "GET" && context.Request.Path == "/calc")
    {
        var firstNumber = context.Request.Query.ContainsKey("firstNumber") &&
                          int.TryParse(context.Request.Query["firstNumber"][0], out var tempFirstNumber)
            ? tempFirstNumber
            : 0;

        var secondNumber = context.Request.Query.ContainsKey("secondNumber") &&
                           int.TryParse(context.Request.Query["secondNumber"][0], out var tempSecondNumber)
            ? tempSecondNumber
            : 0;

        if (context.Request.Query.ContainsKey("operation"))
        {
            var operation = context.Request.Query["operation"][0];
            int? result = operation switch
            {
                "add" => firstNumber + secondNumber,
                "subtract" => firstNumber - secondNumber,
                "multiply" => firstNumber * secondNumber,
                "divide" => secondNumber != 0 ? firstNumber / secondNumber : 0,
                "mod" => secondNumber != 0 ? firstNumber % secondNumber : 0,
                _ => null
            };

            if (result.HasValue)
            {
                await context.Response.WriteAsync(result.Value.ToString());
            }
        }
    }
});

await app.RunAsync();

app.Run();