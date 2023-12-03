namespace Presentation.Users.DataTransferObjects;

public sealed record UpdateUserRequestBody(string DisplayName, List<string>? Roles);
