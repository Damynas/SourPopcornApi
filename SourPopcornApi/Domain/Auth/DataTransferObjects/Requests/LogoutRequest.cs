using Domain.Abstractions.Interfaces;

namespace Domain.Auth.DataTransferObjects.Requests;

public sealed record LogoutRequest(int UserId) : IRequest;
