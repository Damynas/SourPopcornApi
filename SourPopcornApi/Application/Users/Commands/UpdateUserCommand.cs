using Application.Abstractions.Messaging;
using Domain.Users.DataTransferObjects.Requests;
using Domain.Users.Entities;

namespace Application.Users.Commands;

public record UpdateUserCommand(UpdateUserRequest Request) : ICommand<User?>;
