using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();

// Exception handling
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception exception)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An error has occurred.");
        await context.Response.WriteAsync($"Error message: {exception.Message}");
    }
});

app.UseHttpsRedirection();

app.RegisterEndpoints();

app.Run();
