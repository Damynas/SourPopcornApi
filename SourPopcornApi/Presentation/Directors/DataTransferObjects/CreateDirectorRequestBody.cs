namespace Presentation.Directors.DataTransferObjects;

public sealed record CreateDirectorRequestBody(string Name, string Country, DateTime BornOn);
