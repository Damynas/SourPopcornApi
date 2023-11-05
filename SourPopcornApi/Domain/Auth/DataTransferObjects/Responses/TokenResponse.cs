using Domain.Abstractions.Interfaces;

namespace Domain.Auth.DataTransferObjects.Responses;

public sealed record TokenResponse(string AccessToken, string RefreshToken) : IResponse;
