using Application.Abstractions.Messaging;
using Domain.Directors.DataTransferObjects.Requests;
using Domain.Directors.Entities;

namespace Application.Directors.Queries;

public sealed record GetDirectorByIdQuery(GetDirectorByIdRequest Request) : IQuery<Director?>;
