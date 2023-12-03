using Application.Abstractions.Messaging;
using Domain.Auth.DataTransferObjects.Requests;

namespace Application.Auth.Commands;

public record LogoutCommand(LogoutRequest Request) : ICommand;