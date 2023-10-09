using Application.Abstractions.Messaging;
using Domain.Movies.DataTransferObjects.Requests;
using Domain.Movies.Entities;

namespace Application.Movies.Commands;

public record UpdateMovieCommand(UpdateMovieRequest Request) : ICommand<Movie?>;
