using Domain.Abstractions.Interfaces;

namespace Domain.Users.DataTransferObjects.Requests;

public sealed record GetUserByUsernameRequest(string Username) : IRequest;
