using Application.Abstractions.Messaging;
using Domain.Directors.DataTransferObjects.Requests;

namespace Application.Directors.Commands;

public record DeleteDirectorCommand(DeleteDirectorRequest Request) : ICommand;
