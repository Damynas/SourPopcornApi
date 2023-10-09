using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record DeleteVoteRequest(int Id) : IRequest;
