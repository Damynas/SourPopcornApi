using Application.Abstractions.Messaging;
using Domain.Ratings.DataTransferObjects.Requests;
using Domain.Ratings.Entities;

namespace Application.Ratings.Commands;

public record CreateMovieRatingCommand(CreateMovieRatingRequest Request) : ICommand<Rating?>;
