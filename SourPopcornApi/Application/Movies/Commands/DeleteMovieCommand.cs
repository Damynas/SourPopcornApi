using Application.Abstractions.Messaging;
using Domain.Movies.DataTransferObjects.Requests;

namespace Application.Movies.Commands;

public record DeleteMovieCommand(DeleteMovieRequest Request) : ICommand;
