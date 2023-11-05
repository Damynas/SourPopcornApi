using Domain.Abstractions.Interfaces;

namespace Domain.Auth.DataTransferObjects.Requests;
public sealed record LoginRequest(string Username, string Password) : IRequest;
