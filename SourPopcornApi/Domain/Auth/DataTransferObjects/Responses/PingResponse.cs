using Domain.Abstractions.Interfaces;

namespace Domain.Auth.DataTransferObjects.Responses;

public sealed record PingResponse(int UserId, IEnumerable<string> Roles) : IResponse;
