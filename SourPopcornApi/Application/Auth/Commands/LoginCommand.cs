using Application.Abstractions.Messaging;
using Domain.Auth.DataTransferObjects.Requests;
using Domain.Users.Entities;

namespace Application.Auth.Commands;

public record LoginCommand(LoginRequest Request) : ICommand<User?>;
