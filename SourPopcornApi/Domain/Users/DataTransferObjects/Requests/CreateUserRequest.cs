using Domain.Abstractions.Interfaces;

namespace Domain.Users.DataTransferObjects.Requests;

public sealed record CreateUserRequest(string Username, string Password, string DisplayName) : IRequest;
