using Domain.Abstractions.Interfaces;

namespace Domain.Users.DataTransferObjects.Requests;

public sealed record UpdateUserRequest(int Id, string DisplayName) : IRequest;
