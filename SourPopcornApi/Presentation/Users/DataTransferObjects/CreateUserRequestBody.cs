namespace Presentation.Users.DataTransferObjects;

public sealed record CreateUserRequestBody(string Username, string Password, string DisplayName);
