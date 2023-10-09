using Domain.Abstractions.Interfaces;

namespace Domain.Directors.DataTransferObjects.Responses;

public sealed record DirectorResponse(int Id, DateTime CreatedOn, DateTime ModifiedOn, string Name, string Country, DateTime BornOn) : IResponse;
