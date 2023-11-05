namespace Presentation.Auth.DataTransferObjects;

public sealed record RegisterRequestBody(string Username, string Password, string DisplayName);
