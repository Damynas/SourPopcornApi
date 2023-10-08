using Application.Abstractions.Messaging;
using Domain.Users.DataTransferObjects.Requests;
using Domain.Users.Entities;

namespace Application.Users.Queries;

public sealed record GetUserByIdQuery(GetUserByIdRequest Request) : IQuery<User?>;
