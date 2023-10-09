using Application.Abstractions.Messaging;
using Domain.Votes.DataTransferObjects.Requests;

namespace Application.Votes.Commands;

public record DeleteVoteCommand(DeleteVoteRequest Request) : ICommand;
