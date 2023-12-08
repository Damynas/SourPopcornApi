namespace WebApi.Middleware
{
    public static class ErrorHandlingMiddleware
    {
        public static void AddErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            builder.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception exception)
                {
                    context.Response.ContentType = "text/plain";

                    if (exception is BadHttpRequestException)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Request body is incorrect.");
                        return;
                    }

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("An error has occurred.\n");
                    await context.Response.WriteAsync($"Error message: {exception.Message}");
                }
            });
        }
    }
}
