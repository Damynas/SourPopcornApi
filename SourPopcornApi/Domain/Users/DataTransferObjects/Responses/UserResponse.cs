using Domain.Abstractions.Interfaces;

namespace Domain.Users.DataTransferObjects.Responses;

public sealed record UserResponse(int Id, DateTime CreatedOn, DateTime ModifiedOn, string Username, string DisplayName, IEnumerable<string> Roles) : IResponse;
