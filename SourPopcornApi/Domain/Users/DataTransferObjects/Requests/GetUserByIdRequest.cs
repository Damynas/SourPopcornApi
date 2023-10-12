using Domain.Abstractions.Interfaces;

namespace Domain.Users.DataTransferObjects.Requests;

public sealed record GetUserByIdRequest(int UserId) : IRequest;
