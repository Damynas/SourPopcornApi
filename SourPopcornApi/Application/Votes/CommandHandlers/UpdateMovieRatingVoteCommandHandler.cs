﻿using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Votes.Abstractions;
using Application.Votes.Commands;
using Domain.Auth.Constants;
using Domain.Shared;
using Domain.Votes.Entities;

namespace Application.Votes.CommandHandlers;

public class UpdateMovieRatingVoteCommandHandler(IMovieRepository movieRepository, IVoteRepository voteRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMovieRatingVoteCommand, Vote?>
{
    public async Task<Result<Vote?>> Handle(UpdateMovieRatingVoteCommand command, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(command.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        var rating = movie.Ratings.AsEnumerable().SingleOrDefault(x => x.Id == command.Request.RatingId);
        if (rating is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified rating does not exist for specified movie."));

        var vote = rating.Votes.AsEnumerable().SingleOrDefault(x => x.Id == command.Request.VoteId);
        if (vote is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified vote does not exist for specified rating."));

        if (command.Request.UserId != vote.CreatorId && !command.Request.Roles.Contains(Role.Moderator))
            return Result<Vote?>.Failure(null, Error.Forbidden("You are not allowed to update another user's vote."));

        vote.ModifiedOn = DateTime.UtcNow;
        vote.IsPositive = command.Request.IsPositive;

        voteRepository.Update(vote);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result<Vote?>.Success(vote);
    }
}
