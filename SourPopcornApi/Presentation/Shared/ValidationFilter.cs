using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Shared;

public class ValidationFilter<TEntity> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<TEntity>>();
        if (validator is not null)
        {
            var entity = context.Arguments.OfType<TEntity>().FirstOrDefault();
            if (entity is null)
                return TypedResults.Problem("Could not find a type to validate");

            var validation = await validator.ValidateAsync(entity);
            if (!validation.IsValid)
                return TypedResults.UnprocessableEntity(validation.ToDictionary());
        }

        return await next(context);
    }
}


