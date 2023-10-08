using Application.Abstractions.Messaging;
using Domain.Users.DataTransferObjects.Requests;
using Domain.Users.Entities;

namespace Application.Users.Queries;

public sealed record GetUserByUsernameQuery(GetUserByUsernameRequest Request) : IQuery<User?>;

