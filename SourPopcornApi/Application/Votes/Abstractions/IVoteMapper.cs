using Application.Abstractions.Data;
using Domain.Votes.DataTransferObjects.Responses;
using Domain.Votes.Entities;

namespace Application.Votes.Abstractions;

public interface IVoteMapper : IMapper<Vote, VoteResponse> { }
