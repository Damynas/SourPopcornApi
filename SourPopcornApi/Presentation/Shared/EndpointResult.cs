using Domain.Shared;

namespace Presentation.Shared;

public record EndpointResult<TResource>(TResource Resource, IReadOnlyCollection<Link> Links);

public record EndpointResult(IReadOnlyCollection<Link> Links);
