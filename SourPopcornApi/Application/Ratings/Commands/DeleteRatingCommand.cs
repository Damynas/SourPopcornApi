using Application.Abstractions.Messaging;
using Domain.Ratings.DataTransferObjects.Requests;

namespace Application.Ratings.Commands;

public record DeleteRatingCommand(DeleteRatingRequest Request) : ICommand;
