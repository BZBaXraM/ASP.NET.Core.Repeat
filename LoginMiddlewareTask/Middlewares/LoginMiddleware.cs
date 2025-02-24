using Microsoft.AspNetCore.WebUtilities;

namespace LoginMiddlewareTask.Middlewares;

public class LoginMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/" && context.Request.Method == "POST")
        {
            using StreamReader reader = new(context.Request.Body);
            var body = await reader.ReadToEndAsync();

            var queryDict = QueryHelpers.ParseQuery(body);

            string? email = null, password = null;

            if (queryDict.TryGetValue("email", out var value))
            {
                email = Convert.ToString(value[0]);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'email'\n");
            }

            if (queryDict.TryGetValue("password", out var value1))
            {
                password = Convert.ToString(value1[0]);
            }
            else
            {
                if (context.Response.StatusCode == 200)
                    context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'password'\n");
            }

            if (string.IsNullOrEmpty(email) == false && string.IsNullOrEmpty(password) == false)
            {
                bool isValidLogin;

                if (email == "admin@example.com" && password == "admin1234")
                {
                    isValidLogin = true;
                }
                else
                {
                    isValidLogin = false;
                }

                if (isValidLogin)
                {
                    await context.Response.WriteAsync("Successful login\n");
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid login\n");
                }
            }
        }
        else
        {
            await next(context);
        }
    }
}