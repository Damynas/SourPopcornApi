using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record GetVoteByIdRequest(int Id) : IRequest;
