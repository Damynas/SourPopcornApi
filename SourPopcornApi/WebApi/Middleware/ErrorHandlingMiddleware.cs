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
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An error has occurred.");
                    await context.Response.WriteAsync($"Error message: {exception.Message}");
                }
            });
        }
    }
}
