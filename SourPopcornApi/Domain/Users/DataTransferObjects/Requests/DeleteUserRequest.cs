using Domain.Abstractions.Interfaces;

namespace Domain.Users.DataTransferObjects.Requests;

public sealed record DeleteUserRequest(int Id) : IRequest;
