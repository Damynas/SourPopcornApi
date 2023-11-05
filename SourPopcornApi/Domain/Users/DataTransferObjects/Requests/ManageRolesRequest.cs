using Domain.Abstractions.Interfaces;

namespace Domain.Users.DataTransferObjects.Requests;

public sealed record ManageRolesRequest(int UserId, string Role) : IRequest;
