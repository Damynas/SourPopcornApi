using Domain.Abstractions.Interfaces;

namespace Domain.Auth.DataTransferObjects.Requests;
public sealed record RegisterRequest(string Username, string Password, string DisplayName) : IRequest;
