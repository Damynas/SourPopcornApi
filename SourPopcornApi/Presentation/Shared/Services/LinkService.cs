using Application.Abstractions.Services;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Shared.Services;

public class LinkService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor) : ILinkService
{
    public Link Generate(string endpointName, object? routeValues, string rel, string method)
    {
        if (httpContextAccessor.HttpContext is null)
            return new Link(string.Empty, string.Empty, string.Empty);

        return new Link(
            linkGenerator.GetUriByName(
                httpContextAccessor.HttpContext,
                endpointName,
                routeValues),
            rel,
            method);
    }
}
