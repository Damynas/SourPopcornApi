using Application.Abstractions.Messaging;
using Domain.Users.DataTransferObjects.Requests;
using Domain.Users.Entities;

namespace Application.Users.Commands;

public record CreateUserCommand(CreateUserRequest Request) : ICommand<User>;
