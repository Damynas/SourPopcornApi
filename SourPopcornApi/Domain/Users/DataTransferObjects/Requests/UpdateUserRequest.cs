using Domain.Abstractions.Interfaces;

namespace Domain.Users.DataTransferObjects.Requests;

public sealed record UpdateUserRequest(int UserId, string DisplayName, List<string>? Roles) : IRequest;
