using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record UpdateVoteRequest(int Id, bool IsPositive) : IRequest;
