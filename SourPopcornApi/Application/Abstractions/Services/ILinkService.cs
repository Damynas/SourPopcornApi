using Domain.Shared;

namespace Application.Abstractions.Services;

public interface ILinkService
{
    Link Generate(string endpointName, object? routeValues, string rel, string method);
}
