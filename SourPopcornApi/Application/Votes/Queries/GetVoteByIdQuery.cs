using Application.Abstractions.Messaging;
using Domain.Votes.DataTransferObjects.Requests;
using Domain.Votes.Entities;

namespace Application.Votes.Queries;

public sealed record GetVoteByIdQuery(GetVoteByIdRequest Request) : IQuery<Vote?>;
