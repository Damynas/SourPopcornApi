﻿using FluentValidation;
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
                return TypedResults.UnprocessableEntity(TransformValidationErrors(validation.ToDictionary()));
        }

        return await next(context);
    }

    private static List<object> TransformValidationErrors(IDictionary<string, string[]> validationErrors)
    {
        return validationErrors.Select(entry => new ValidationResponse(entry.Key, string.Join(" ", entry.Value)))
            .Cast<object>().ToList();
    }
}

public sealed record ValidationResponse(string PropertyName, string Value);
