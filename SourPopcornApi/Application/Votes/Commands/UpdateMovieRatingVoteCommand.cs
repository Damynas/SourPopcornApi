using Application.Abstractions.Messaging;
using Domain.Votes.DataTransferObjects.Requests;
using Domain.Votes.Entities;

namespace Application.Votes.Commands;

public record UpdateMovieRatingVoteCommand(UpdateMovieRatingVoteRequest Request) : ICommand<Vote?>;
