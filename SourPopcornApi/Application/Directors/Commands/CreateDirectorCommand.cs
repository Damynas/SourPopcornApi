using Application.Abstractions.Messaging;
using Domain.Directors.DataTransferObjects.Requests;
using Domain.Directors.Entities;

namespace Application.Directors.Commands;

public record CreateDirectorCommand(CreateDirectorRequest Request) : ICommand<Director>;
