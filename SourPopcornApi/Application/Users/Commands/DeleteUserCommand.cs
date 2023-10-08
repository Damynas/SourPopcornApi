using Application.Abstractions.Messaging;
using Domain.Users.DataTransferObjects.Requests;

namespace Application.Users.Commands;

public record DeleteUserCommand(DeleteUserRequest Request) : ICommand;
